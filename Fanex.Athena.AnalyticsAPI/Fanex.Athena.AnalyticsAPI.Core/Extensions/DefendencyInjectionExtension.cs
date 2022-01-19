using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
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