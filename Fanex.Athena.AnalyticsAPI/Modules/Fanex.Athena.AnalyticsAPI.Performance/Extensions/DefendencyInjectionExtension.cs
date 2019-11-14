using Fanex.Athena.AnalyticsAPI.Performance.Mappers;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Performance.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDIPerformance(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories()
                .AddMappers();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrencyPerformanceService, CurrencyPerformanceService>();
            services.AddTransient<IOverviewPerformanceService, OverviewPerformanceService>();
            services.AddTransient<IProductPerformanceService, ProductPerformanceService>();
            services.AddTransient<IPlatformPerformanceService, PlatformPerformanceService>();
            services.AddTransient<IBettypePerformanceService, BettypePerformanceService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPerformanceRepository, PerformanceRepository>();

            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddTransient<IOverviewPerformanceVMMapper, OverviewPerformanceVMMapper>();
            services.AddTransient<ICurrencyPerformanceVMMapper, CurrencyPerformanceVMMapper>();
            services.AddTransient<IProductPerformanceVMMapper, ProductPerformanceVMMapper>();
            services.AddTransient<IPlatformPerformanceVMMapper, PlatformPerformanceVMMapper>();
            services.AddTransient<IBettypePerformanceVMMapper, BettypePerformanceVMMapper>();

            return services;
        }
    }
}