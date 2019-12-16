using Fanex.Athena.AnalyticsAPI.Core.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ONEbook.UM.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }

        public static IApplicationBuilder UseOneAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OneAuthenticationHandler>();
        }

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IHostingEnvironment env, string apiName)
        {
            if (env.IsDevelopment())
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

        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder builder, IHostingEnvironment env, string apiName)
        {
            if (env.IsDevelopment())
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiName} V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            return builder;
        }

        public static IApplicationBuilder UseUM(this IApplicationBuilder builder)
        {
            builder.UseUMHandler((ctx, umInfo) =>
            {
                return ctx.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    msg = $"System is currently unavailable due to scheduled maintenance. This downtime will last from {umInfo.From} to {umInfo.To}.",
                    isSuccess = false
                }));
            });

            return builder;
        }
    }
}