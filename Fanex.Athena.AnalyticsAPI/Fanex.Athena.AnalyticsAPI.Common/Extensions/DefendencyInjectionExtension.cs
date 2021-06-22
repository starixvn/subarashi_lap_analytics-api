using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.RemotingRef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendencyInjectionExtension
    {
        private const string BRIDGE_TOKEN_KEY = "LGAPI_TOKEN_KEY";

        public static IServiceCollection AddDICommon(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddServices(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContextService, ContextService>();
            services.AddSingleton<IDaxQueryContentService, DaxQueryContentService>();
            services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddTransient<ISsasBridgeService, SsasBridgeService>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ISqlService, SqlService>();
            services.AddSingleton<ILangResourceService, LangResourceService>();
            services.AddSingleton<ITranslationService, TranslationService>();
            services.AddSingleton<IHandleRawData, RawDataService>();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<ICommonService, CommonService>();

            var setting = new SSASSetting();
            configuration.GetSection("SSASSetting").Bind(setting);
            services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
            {
                client.BaseAddress = new Uri(setting.LGAPIHost);
                client.DefaultRequestHeaders.Add(BRIDGE_TOKEN_KEY, setting.LGAPIToken);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseProxy = false
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(setting.ConnectionExpireTime));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}