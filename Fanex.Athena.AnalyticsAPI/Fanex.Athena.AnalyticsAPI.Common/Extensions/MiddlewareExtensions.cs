using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Fanex.Athena.AnalyticsAPI.Common.Middleware;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandling>();
        }

        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPFilter>();
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string title = null, string description = null)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = string.IsNullOrWhiteSpace(title) ? title : "LAP - API Analytics",
                    Description = string.IsNullOrWhiteSpace(description) ? description : "LAP - API Analytics"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LAP - API Analytics V1");
            });

            return builder;
        }
    }
}
