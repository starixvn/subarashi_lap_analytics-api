using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendenctInjectExtension
    {
        public static IServiceCollection AddDICommon(this IServiceCollection services)
        {
            services.AddServices();
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IContextService, ContextService>();
            services.AddTransient<ISsasService, SsasService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDAXQueryRepository, DAXQueryRepository>();

            return services;
        }
    }
}