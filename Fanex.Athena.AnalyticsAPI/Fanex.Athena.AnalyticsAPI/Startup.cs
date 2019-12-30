using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.ExtService.Common.Configuration;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry.RavenClient;
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
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Startup
    {
        private const string API_NAME = "LAP - Analytics API";

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

            services.AddSwaggerDoc(Env, API_NAME);

            services.Configure<SSASSetting>(Configuration.GetSection("SSASSetting"));

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
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

            SetupLogging(services);
            SetupSqlProvider();
            SetupAuthentication(services);

            Performance.Extensions.DefendencyInjectionExtension.AddDIPerformance(services);
            Common.Extensions.DefendencyInjectionExtension.AddDICommon(services, Configuration);

            services.AddUMService(Configuration.GetSection("ONEbook.UM").Get<UMClientConfiguration>());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
            app.UseUM();
            app.UseErrorHandler(Env);
            app.UseOneAuth();
            app.UseMvc();
        }

        private void SetupLogging(IServiceCollection services)
        {
            var loggingSetting = Configuration.GetSection("LoggingSetting").Get<LoggingSetting>();

            var logEngines = new List<ILogEngine>(2);
            if (loggingSetting.RabbitEngine != null)
            {
                logEngines.Add(new RabbitMQLogging(loggingSetting.RabbitEngine.ConnectionString));
            }
            if (loggingSetting.SentryEngine != null)
            {
                logEngines.Add(new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(loggingSetting.SentryEngine.BackendDsn)
                }));
            }

            LogManager
             .SetDefaultLogCategory(loggingSetting.Category)
             .Use(logEngines);
            services.AddSingleton(_ => Logger.Log);
        }

        private void SetupSqlProvider()
        {
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>
            {
                {
                    "DBEXT.bodb_dwrs.bodbLicenseeNet",
                    new ConnectionConfiguration("DBEXT.bodb_dwrs.bodbLicenseeNet",
                        Configuration.GetConnectionString("DBEXT.bodb_dwrs.bodbLicenseeNet"))
                }
            };

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "~/SpConfig")
                .Use(connectionStrings)
                .Run();
        }

        private void SetupAuthentication(IServiceCollection services)
        {
            services.AddSingleton<IStorageProvider, SqlServerProvider>(x =>
            {
                var connection = GetOneBookSecurityConnection();
                return new SqlServerProvider(connection);
            });

            services.AddSingleton<IAuth<DefaultAuthInfo<Permission, dynamic>>, Auth<DefaultAuthInfo<Permission, dynamic>>>();
        }

        private string GetOneBookSecurityConnection()
        {
            var encryptedConnectionString = Configuration.GetConnectionString("LAP.OneBookSecurity");
            return Cryptography.DecryptConnectionString(encryptedConnectionString);
        }
    }
}