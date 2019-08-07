using Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services;
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
            services.AddScoped<IPerformanceService, PerformanceService>();
            services.AddScoped<IBetTypePerformanceService, BetTypePerformanceService>();
            services.AddScoped<ICurrencyPerformanceService, CurrencyPerformanceService>();
            services.AddScoped<IOverviewPerformanceService, OverviewPerformanceService>();
            services.AddScoped<IPlatformPerformanceService, PlatformPerformanceService>();
            services.AddScoped<IProductPerformanceService, ProductPerformanceService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}