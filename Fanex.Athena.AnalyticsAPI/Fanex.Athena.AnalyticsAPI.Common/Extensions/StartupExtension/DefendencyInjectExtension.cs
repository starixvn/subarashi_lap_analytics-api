using Fanex.Athena.AnalyticsAPI.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DefendencyInjectExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IContextService, ContextService>();
            return services;
        }
    }
}