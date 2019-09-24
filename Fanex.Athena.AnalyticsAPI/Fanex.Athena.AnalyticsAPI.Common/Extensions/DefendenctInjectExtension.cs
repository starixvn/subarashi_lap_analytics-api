using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendenctInjectExtension
    {
        public static IServiceCollection AddDICommon(this IServiceCollection services)
        {
            services
                .AddServices()
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IContextService, ContextService>();
            services.AddTransient<IDaxQueryContentService, DaxQueryContentService>();
            services.AddTransient<ISsasBridgeService, SsasBridgeService>();
            services.AddTransient<ICalculationService, CalculationService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDynamicRepository, DynamicRepository>();
            return services;
        }
    }
}