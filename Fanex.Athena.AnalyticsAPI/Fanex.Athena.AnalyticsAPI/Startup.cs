using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Extensions.StartupExtension;
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

            services.AddCoreServices();
            services.AddDI();

            services.BuildServiceProvider().SetServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIPFilter();
            app.UseHttpsRedirection();

            app.UseErrorHandler();

            app.UsePathBase(PathBase);
            app.UseContextService();
            app.UseSwaggerDoc(Env, ApiName, PathBase);

            app.UseMvc();
        }
    }
}