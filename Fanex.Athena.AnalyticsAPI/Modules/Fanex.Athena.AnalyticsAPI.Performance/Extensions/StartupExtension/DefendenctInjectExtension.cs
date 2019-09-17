using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Performance.Extensions
{
    public static class DefendenctInjectExtension
    {
        public static IServiceCollection AddDIPerformance(this IServiceCollection services)
        {
            services.AddServices();
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPerformanceService, PerformanceService>();
            services.AddTransient<IBetTypePerformanceService, BetTypePerformanceService>();
            services.AddTransient<ICurrencyPerformanceService, CurrencyPerformanceService>();
            services.AddTransient<IOverviewPerformanceService, OverviewPerformanceService>();
            services.AddTransient<IPlatformPerformanceService, PlatformPerformanceService>();
            services.AddTransient<IProductPerformanceService, ProductPerformanceService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IOverviewPerformanceRepo, OverviewPerformanceRepo>();

            return services;
        }
    }
}