using Fanex.Athena.AnalyticsAPI.Common.Middleware;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.Configurations;
using ONEBook.UM.Extensions;
using ONEBook.UM.Extensions.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }

        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPFilter>();
        }

        public static IApplicationBuilder UseOneAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OneAuthHandler>();
        }

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IHostingEnvironment env, string apiName)
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

            return services;
        }

        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder builder, IHostingEnvironment env, string apiName)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiName} V1");
                c.RoutePrefix = string.Empty;
            });

            return builder;
        }

        public static IServiceCollection AddUMHandler(this IServiceCollection services, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsEnvironment("UAT") || env.IsEnvironment("PROD"))
            {
                services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
                services.AddUMService(configuration.GetSection("ONEbook.UM").Get<UMClientConfiguration>());
            }

            return services;
        }

        public static IApplicationBuilder UseUMHandler(this IApplicationBuilder builder, IHostingEnvironment env)
        {
            if(env.IsEnvironment("UAT") || env.IsEnvironment("PROD"))
            {
                builder.UseMiddleware<UMHandler>();
            }

            return builder;
        }

    }
}