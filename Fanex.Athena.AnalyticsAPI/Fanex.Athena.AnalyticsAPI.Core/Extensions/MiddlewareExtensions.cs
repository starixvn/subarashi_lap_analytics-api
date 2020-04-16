using Fanex.Athena.AnalyticsAPI.Core.Filters;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ONEbook.UM.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                builder.UseMiddleware<ErrorHandler>();
            }
            return builder;
        }

        public static IApplicationBuilder UseWhoAmIHealthCheck(this IApplicationBuilder builder)
        {
            return builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = _ => true,
                ResponseWriter = async (context, result) =>
                {
                    context.Response.ContentType = "application/json";
                    var json = new JObject(
                         new JProperty("status", result.Status.ToString()),
                         new JProperty("results", new JObject(result.Entries.Select(pair =>
                             new JProperty(pair.Key, new JObject(
                                 new JProperty("status", pair.Value.Status.ToString()),
                                 new JProperty("description", pair.Value.Description),
                                 new JProperty("data", new JObject(pair.Value.Data.Select(
                                     p => new JProperty(p.Key, p.Value))))))))));
                    await context.Response.WriteAsync(
                        json.ToString(Formatting.Indented));
                }
            });
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

                    c.OperationFilter<OneAuthHeaderFilter>();

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