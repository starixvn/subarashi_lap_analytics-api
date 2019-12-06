using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.ExtService.Common.Configuration;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.RemotingRef;
using LAP.ONEBook.Security;
using LAP.ONEBook.Security.StorageProvider.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.Configurations;
using ONEbook.UM.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Security.Claims;
using System.Security.Principal;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Startup
    {
        private const string API_NAME = "LAP - Analytics API";
        private const string DEFAULT_SITE_NAME = "12Bet"; //only development

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            RefDataTypeContainer
                .ConnectTo(new Uri(Configuration["Fanex.RemotingRef.Service.Url"]))
                .FromApplication("LAP_Analytics_API");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<IConfiguration>(Configuration);
            services.AddSingleton<IRemotingRefService, RemotingRefService>();

            services.AddSwaggerDoc(Env, API_NAME);

            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            services.Configure<SSASSetting>(Configuration.GetSection("SSASSetting"));

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

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
            Common.Extensions.DefendencyInjectionExtension.AddDICommon(services, Configuration);
            services.AddSingleton<IStorageProvider, SqlServerProvider>(x =>
            {
                var connection = GetOneBookSecurityConnection();
                return new SqlServerProvider(connection);
            });

            services.AddUMService(Configuration.GetSection("ONEbook.UM").Get<UMClientConfiguration>());
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseCors("DefaultPolicy");

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("zh-CN")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseResponseCompression();
            app.UseSwaggerDoc(Env, API_NAME);
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.Use(async (context, next) =>
                {
                    context.User = new GenericPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, DEFAULT_SITE_NAME) }),
                        new[] { "" });

                    await next.Invoke();
                });
            }
            else
            {
                app.UseErrorHandler();
                app.UseOneAuth();
            }
            app.UseUM();

            app.UseMvc();
        }
    }
}