using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.RemotingRef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddDICommon(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddServices(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContextService, ContextService>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ISqlService, SqlService>();
            services.AddSingleton<ILangResourceService, LangResourceService>();
            services.AddSingleton<ITranslationService, TranslationService>();
            services.AddSingleton<IHandleRawData, RawDataService>();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<ICommonService, CommonService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}