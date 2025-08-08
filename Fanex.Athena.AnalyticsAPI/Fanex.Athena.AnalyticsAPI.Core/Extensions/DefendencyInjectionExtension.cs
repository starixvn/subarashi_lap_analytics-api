namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDICore(this IServiceCollection services)
        {
            return services
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthInfoService, AuthInfoService>();
            services.AddSingleton<IMetaModelBindingService, MetaModelBindingService>();

            return services;
        }
    }
}