using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Core.Handlers;
using Fanex.Athena.AnalyticsAPI.Core.Models;
using Fanex.Athena.AnalyticsAPI.Core.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using LAP.ONEBook.Security;
using LAP.ONEBook.Security.StorageProvider.SqlServer;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<AutoBindingMetaModelAttribute>();
            services.AddScoped<IAuthInfoService, AuthInfoService>();
            services.AddSingleton<IMetaModelBindingService, MetaModelBindingService>();

            return services;
        }

        public static IServiceCollection AddONEBookAuthentication(this IServiceCollection services, string securityConnectionString, Action<ONEBookAuthenticationOptions> configureOptions)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = AuthConst.ONEBookScheme;
                option.DefaultAuthenticateScheme = AuthConst.ONEBookScheme;
                option.DefaultChallengeScheme = AuthConst.ONEBookScheme;
            }).AddScheme<ONEBookAuthenticationOptions, ONEBookAuthenticationHandler>(AuthConst.ONEBookScheme, configureOptions);

            services.AddSingleton<IStorageProvider, SqlServerProvider>(x =>
            {
                return new SqlServerProvider(securityConnectionString);
            });

            services.AddSingleton<IAuth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>, Auth<DefaultAuthInfo<Permission, CurrencyAgentInfo>>>();

            return services;
        }
    }
}