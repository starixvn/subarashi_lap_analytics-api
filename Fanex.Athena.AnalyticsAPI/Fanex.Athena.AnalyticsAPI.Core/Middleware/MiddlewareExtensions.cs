using Microsoft.AspNetCore.Builder;

namespace Fanex.Athena.AnalyticsAPI.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPFilter>();
        }

     
    }
}
