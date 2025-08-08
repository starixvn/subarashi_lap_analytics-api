namespace Fanex.Athena.AnalyticsAPI.Summary.Extensions
{
    using Common.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDISummary(this IServiceCollection services)
        {
            return services
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ILicenseeDataPlatformService, LicenseeDataPlatformService>();
            services.AddTransient<ISingleValueService, SingleValueService>();
            services.AddTransient<ICommonService, CommonService>();

            return services;
        }
    }
}