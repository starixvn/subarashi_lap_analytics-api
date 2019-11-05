using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddTransient<ISsasBridgeService, SsasBridgeService>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ISqlService, SqlService>();

            var setting = new SSASSetting();
            configuration.GetSection("SSASSetting").Bind(setting);
            services.AddHttpClient<ISsasBridgeService, SsasBridgeService>(client =>
            {
                client.BaseAddress = new Uri(setting.BridgeHost);
                client.DefaultRequestHeaders.Add(BRIDGE_TOKEN_KEY, setting.AuthKey);
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