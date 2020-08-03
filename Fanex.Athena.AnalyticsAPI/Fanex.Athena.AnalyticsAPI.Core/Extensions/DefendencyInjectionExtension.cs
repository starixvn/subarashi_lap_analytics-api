using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.ONEBookAuth;
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

        public static IServiceCollection AddONEBookAuthentication(this IServiceCollection services, Action<ONEBookAuthenticationOptions> configureOptions)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = AuthConst.ONEBookScheme;
                option.DefaultAuthenticateScheme = AuthConst.ONEBookScheme;
                option.DefaultChallengeScheme = AuthConst.ONEBookScheme;
            }).AddScheme<ONEBookAuthenticationOptions, ONEBookAuthenticationHandler>(AuthConst.ONEBookScheme, configureOptions)
              .Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ONEBookAuthenticationOptions>, PostConfigureONEBookAuthenticationOptions>());

            return services;
        }
    }
}