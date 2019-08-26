using Fanex.Athena.AnalyticsAPI.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    public static class DefendencyInjectExtension
    {
        public static IServiceCollection AddDICore(this IServiceCollection services)
        {
            services.AddServices();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IContextService, ContextService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}