using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Configuration;
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
            services.AddTransient<IDaxQueryService, DaxQueryService>();
            services.AddTransient<ISsasBridgeService, SsasBridgeService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDynamicRepository, DynamicRepository>();
            return services;
        }
    }
}