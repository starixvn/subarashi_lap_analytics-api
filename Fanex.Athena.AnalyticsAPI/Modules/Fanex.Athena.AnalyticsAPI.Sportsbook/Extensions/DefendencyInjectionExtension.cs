
using Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDIPerformance(this IServiceCollection services)
        {
            return services
                .AddRepositories()
                .AddMappers();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ISportsbookRepository, SportsbookRepository>();

            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddTransient<ISportsbookMapper, SportsbookMapper>();
            return services;
        }
    }
}