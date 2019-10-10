using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using LAP.ONEBook.Security;
using LAP.ONEBook.Security.StorageProvider.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Startup
    {
        private const string ApiName = "LAP - Analytics API";
        private const string PathBase = "/api";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDoc(Env, ApiName);

            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            services.Configure<SSASSetting>(Configuration.GetSection("SSASSetting"));

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddHealthChecks();
            services.AddMemoryCache();
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                {
                     "application/json"
                 };
                options.EnableForHttps = true;
                options.MimeTypes = MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
            });

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            LogManager
            .SetDefaultLogCategory(appSettings.LogCategory)
            .Use(new List<ILogEngine>
             {
                new RabbitMQLogging(appSettings.RabbitMQ)
             });

            var conns = GetConnectionString();
            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(resourcePath: "~/SpConfig")
              .Use(conns)
              .Run();

            Performance.Extensions.DefendencyInjectionExtension.AddDIPerformance(services);
            Common.Extensions.DefendencyInjectionExtension.AddDICommon(services);
            services.AddSingleton<IStorageProvider, SqlServerProvider>(x =>
            {
                var connection = GetOneBookSecurityConnection();
                return new SqlServerProvider(connection);
            }
            );

        }

        private string GetOneBookSecurityConnection()
        {
            var encryptedConnectionString = Configuration.GetConnectionString("LAP.OneBook");
            return Cryptography.DecryptConnectionString(encryptedConnectionString);
        }

        private Dictionary<string, ConnectionConfiguration> GetConnectionString()
        {
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>
            {
                {
                    "DBEXT.bodb_dwrs.bodbLicenseeNet",
                    new ConnectionConfiguration("DBEXT.bodb_dwrs.bodbLicenseeNet",
                        Configuration.GetConnectionString("DBEXT.bodb_dwrs.bodbLicenseeNet"))
                }
            };
            return connectionStrings;
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseOneAuth();
                app.UseErrorHandler();
            }
            app.UseCors("DefaultPolicy");
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UsePathBase(PathBase);
            app.UseSwaggerDoc(Env, ApiName, PathBase);
            app.UseMvc();
        }
    }
}