using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Configuration;
using Fanex.Athena.AnalyticsAPI.Controllers;
using Fanex.Athena.AnalyticsAPI.Core.Extensions;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fanex.Athena.AnalyticsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            RefDataTypeContainer
               .ConnectTo(new Uri(Configuration["Fanex.RemotingRef.Service.Url"]))
               .FromApplication("LAP_Analytics_API");

            SetupMediatR(services);

            services.Configure<IConfiguration>(Configuration);

            services.Configure<SSASSetting>(Configuration.GetSection("SSASSetting"));
            services.Configure<StaticResourceConfig>(Configuration.GetSection("StaticResource"));
            services.Configure<IdentityServerConfig>(Configuration.GetSection("IdentityServer"));

            services.AddCorsPortalSite(Configuration);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddHealthChecks().AddCheck<WhoAmIHealthCheck>("WhoAmIHealthCheck");
            services.AddUnderMaintenance(Configuration);
            SetupSqlProvider();
            ConfigRequestLocalization(services);

            var instrospecCacheDuration = int.TryParse(Configuration["IdentityServer:CacheDuration"], out int cache) ? cache : 0;
            services.AddAuthentication()
               .AddOAuth2Introspection(options =>
               {
                   options.Authority = Configuration["IdentityServer:Host"];
                   options.ClientId = Configuration["IdentityServer:CredentialIntrospection:ClientId"];
                   options.ClientSecret = Configuration["IdentityServer:CredentialIntrospection:ClientSecret"];
                   options.SaveToken = true;
                   options.SkipTokensWithDots = true;
                   options.EnableCaching = instrospecCacheDuration > 0;
                   options.CacheKeyPrefix = "OAuth2Introspection:";
                   options.CacheDuration = TimeSpan.FromMinutes(instrospecCacheDuration);
                   options.DiscoveryPolicy.RequireHttps = false;

                   options.TokenRetriever = TokenRetrieval.FromAuthorizationHeader();
               });

            services.AddHttpClient<AuthController>()
            .SetHandlerLifetime(TimeSpan.FromDays(1));

            Performance.Extensions.DefendencyInjectionExtension.AddDIPerformance(services);
            Sportsbook.Extensions.DefendencyInjectionExtension.AddDIPerformance(services);
            Summary.Extensions.DefendencyInjectionExtension.AddDISummary(services);
            Common.Extensions.DefendencyInjectionExtension.AddDICommon(services, Configuration);
            Core.Extensions.DefendencyInjectionExtension.AddDICore(services);

            services.AddHttpsRedirection(options =>
            {
                const int httpsPort = 443;
                options.HttpsPort = httpsPort;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthCheck();
            app.UseForwardedHeaders();

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWhoAmICheck();

            if (!HostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticResource(HostingEnvironment.IsDevelopment());
            app.UseUnderMaintenance();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => await context.Response.WriteAsync("LAP Analytics is running"));
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void SetupMediatR(IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Fanex.Athena.AnalyticsAPI"));
            services.AddMediatR(assemblies.ToArray());
        }

        private void SetupSqlProvider()
        {
            var listConnectionName = Configuration.GetSection("ConnectionStrings").GetChildren().Select(_ => _.Key);
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>(listConnectionName
                .Select(connectionName => new KeyValuePair<string, ConnectionConfiguration>(connectionName,
                    new ConnectionConfiguration(connectionName, Configuration.GetConnectionString(connectionName)))
                ));

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "~/SpConfig/FanexSpName.xml")
                .Use(connectionStrings)
                .Run();
        }

        private static void ConfigRequestLocalization(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                const string EnCultureName = "en-US";
                var enCulture = new CultureInfo(EnCultureName)
                {
                    DateTimeFormat = new DateTimeFormatInfo
                    {
                        ShortDatePattern = "M/d/yyyy",
                        ShortTimePattern = "h:mm tt",
                        LongDatePattern = "MM/dd/yyyy",
                        LongTimePattern = "h:mm:ss tt"
                    }
                };

                options.DefaultRequestCulture = new RequestCulture(EnCultureName);
                options.SupportedCultures = new List<CultureInfo>
                {
                    enCulture
                };
                options.SupportedUICultures = new List<CultureInfo>
                {
                    enCulture,
                    new CultureInfo("zh-CN"),
                    new CultureInfo("vi-VN"),
                    new CultureInfo("id-ID"),
                    new CultureInfo("th-TH"),
                };

                var languageCodeMappers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
                {
                    { "en", "en-US" },
                    { "cs", "zh-CN" },
                    { "vn", "vi-VN" },
                    { "id", "id-ID" },
                    { "th", "th-TH" },
                };

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CustomRequestCultureProvider(context =>
                    {
                        var langCode = context.User.FindFirst("Language")?.Value ?? "en";
                        var culture = languageCodeMappers.GetValueOrDefault(langCode, "en-US");
                        return Task.FromResult(new ProviderCultureResult(culture: EnCultureName, uiCulture: culture));
                    })
                };
            });
        }
    }
}