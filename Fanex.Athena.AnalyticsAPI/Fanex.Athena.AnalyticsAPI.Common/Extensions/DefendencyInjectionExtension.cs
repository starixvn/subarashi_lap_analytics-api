using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDICommon(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IContextService, ContextService>();
            services.AddTransient<IDaxQueryContentService, DaxQueryContentService>();
            services.AddTransient<ISsasBridgeService, SsasBridgeService>();
            services.AddTransient<ICalculationService, CalculationService>();

            services.AddHttpClient<ISsasBridgeService, SsasBridgeService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}