using Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services.BetType;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions.StartupExtension
{
    public static class DefendencyInjectExtension
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            services.AddServices();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBetTypePerformanceService, BetTypePerformanceService>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}