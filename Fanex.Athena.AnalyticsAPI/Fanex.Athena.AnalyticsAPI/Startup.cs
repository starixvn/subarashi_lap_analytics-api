using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Configuration;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.RemotingRef;
using LAP.ONEBook.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.Configurations;
using ONEbook.UM.Extensions;
using SharpRaven;
using System;
using System.Collections.Generic;
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

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<IConfiguration>(Configuration);

            services.AddSwaggerDoc(Env, API_NAME);

            services.Configure<SSASSetting>(Configuration.GetSection("SSASSetting"));

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                var preflightMaxAge30InDay = TimeSpan.FromDays(30);

                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .SetPreflightMaxAge(preflightMaxAge30InDay);
            }));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
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

            services.AddHealthChecks().AddCheck<WhoAmIHealthCheck>("WhoAmIHealthCheck");

            SetupLogging(services);
            SetupSqlProvider();

            services.AddONEBookAuthentication(options =>
            {
                var isDev = Env.IsDevelopment();
                if (isDev && Configuration.GetValue<bool>("AllowBypassAuthentication"))
                {
                    var section = Configuration.GetSection("FakeUserData");
                    var fakeData = section.Get<DefaultAuthInfo<Permission, CurrencyAgentInfo>>();
                    options.FakeData = () => fakeData;
                    options.AllowBypass = true;
                }
                else
                {
                    options.SecurityConnectionString = GetOneBookSecurityConnection();
                }
            });

            Performance.Extensions.DefendencyInjectionExtension.AddDIPerformance(services);
            Summary.Extensions.DefendencyInjectionExtension.AddDISummary(services);
            Common.Extensions.DefendencyInjectionExtension.AddDICommon(services, Configuration);
            Core.Extensions.DefendencyInjectionExtension.AddDICore(services);

            services.AddUMService(Configuration.GetSection("ONEbook.UM").Get<UMClientConfiguration>());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseWhoAmIHealthCheck();
            app.UseCors("DefaultPolicy");
            app.UseLocalization();
            app.UseResponseCompression();
            app.UseSwaggerDoc(Env, API_NAME);
            app.UseUM();
            app.UseStatusCodeResponse();
            app.UseErrorHandler(Env);
            app.UseAuthentication();
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
                },
                {
                    "DBEXT.bodb02.bodbLicenseeNet",
                    new ConnectionConfiguration("DBEXT.bodb02.bodbLicenseeNet",
                        Configuration.GetConnectionString("DBEXT.bodb02.bodbLicenseeNet"))
                },
                {
                    "DBACC.bodb02.bodbLicenseeNet",
                    new ConnectionConfiguration("DBACC.bodb02.bodbLicenseeNet",
                        Configuration.GetConnectionString("DBACC.bodb02.bodbLicenseeNet"))
                }
            };

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "~/SpConfig")
                .Use(connectionStrings)
                .Run();
        }

        private string GetOneBookSecurityConnection()
        {
            var encryptedConnectionString = Configuration.GetConnectionString("LAP.OneBookSecurity");
            return Cryptography.DecryptConnectionString(encryptedConnectionString);
        }
    }
}