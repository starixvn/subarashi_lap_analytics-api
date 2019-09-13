using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Performance.Extensions;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Startup
    {
        private static readonly string ApiName = "LAP - Analytics API";
        private static readonly string PathBase = "/api";

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

            services.Configure<ApplicationOptions>(Configuration);

            services.AddHealthChecks();

            services.AddMemoryCache();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
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

        }
        private Dictionary<string, ConnectionConfiguration> GetConnectionString()
        {
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>
            {
                {
                    "DBEXT.bodb02.bodbLicenseeNet",
                    new ConnectionConfiguration("DBEXT.bodb02.bodbLicenseeNet",
                Configuration.GetConnectionString("DBEXT.bodb02.bodbLicenseeNet"))

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
                app.UseErrorHandler();

            }

            app.UseIPFilter();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UsePathBase(PathBase);
            app.UseSwaggerDoc(Env, ApiName, PathBase);
            app.UseMvc();
        }
    }
}