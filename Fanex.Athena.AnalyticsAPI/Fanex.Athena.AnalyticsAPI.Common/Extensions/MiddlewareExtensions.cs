using Fanex.Athena.AnalyticsAPI.Common.Middleware;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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

        public static IApplicationBuilder UseContextService(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                ServiceProviderExtensions.GetRequiredService<IContextService>().Context = context;
                await next.Invoke();
            });
        }

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IHostingEnvironment env, string apiName)
        {
            if (!env.IsProduction())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = apiName,
                    });

                    c.DescribeStringEnumsInCamelCase();
                    c.DescribeAllEnumsAsStrings();
                });
            }

            return services;
        }

        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder builder, IHostingEnvironment env, string apiName, string pathBase)
        {
            if (!env.IsProduction())
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(pathBase + "/swagger/v1/swagger.json", $"{apiName} V1");
                });
            }

            return builder;
        }
    }
}