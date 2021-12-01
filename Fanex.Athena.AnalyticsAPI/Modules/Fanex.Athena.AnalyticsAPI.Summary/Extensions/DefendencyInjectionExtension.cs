using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Common.Utility;
using Fanex.Athena.AnalyticsAPI.Summary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Summary.Extensions
{
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
            services.AddTransient<ICustomMemoryCacheService, CustomMemoryCacheService>();
            services.AddTransient<ICustomMemoryCache, CustomMemoryCache>();

            return services;
        }
    }
}