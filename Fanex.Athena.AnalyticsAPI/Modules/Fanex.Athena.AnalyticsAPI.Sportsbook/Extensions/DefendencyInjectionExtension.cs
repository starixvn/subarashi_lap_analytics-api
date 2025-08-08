namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Extensions
{
    using Mappers;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;

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