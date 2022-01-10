using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddCorsPortalSite(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedCrossDomains = configuration.GetSection("AllowedCrossDomains")
                .Get<IEnumerable<string>>();

            var preflightMaxAge30InDay = TimeSpan.FromDays(30);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowed(domain =>
                                      {
                                          if (allowedCrossDomains == null || !allowedCrossDomains.Any())
                                          {
                                              return true;
                                          }

                                          var clientDomain = new Uri(domain).Authority;
                                          var isValid = allowedCrossDomains.Any(allowedCrossDomain => clientDomain.EndsWith(allowedCrossDomain, StringComparison.OrdinalIgnoreCase));
                                          return isValid;
                                      })
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .SetPreflightMaxAge(preflightMaxAge30InDay);
                                  });
            });
        }


        public static void AddUnderMaintenance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSession();
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            const string RequestedWithHeader = "X-Requested-With";
            const string XmlHttpRequest = "XMLHttpRequest";
            bool IsAjaxRequest(HttpRequest request)
                => request.Headers.TryGetValue(RequestedWithHeader, out StringValues headerValue)
                   && string.Equals(headerValue.FirstOrDefault(), XmlHttpRequest, StringComparison.OrdinalIgnoreCase);

            builder.UseWhen(context => IsAjaxRequest(context.Request),
               builder => builder.UseStatusCodePages(async context =>
               {
                   var response = context.HttpContext.Response;
                   response.ContentType = "application/json";
                   var errorObject = new
                   {
                       statusCode = response.StatusCode,
                       msg = "An error occurred while processing your request",
                       isSuccess = false
                   };
                   await response.WriteAsync(JsonConvert.SerializeObject(errorObject));
               }));

            builder.UseMiddleware<ErrorHandler>();

            return builder;
        }

        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping"))
                {
                    return;
                }
                await next();
            });


            builder.UseHealthChecks("/health", new HealthCheckOptions
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

            return builder;
        }

        public static void UseStaticResource(this IApplicationBuilder app, bool isDev)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int OneYear = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = isDev ?
                    "no-cache" : "public,max-age=" + OneYear;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.AccessControlAllowOrigin] = "*";
                }
            });
        }
        public static void UseUnderMaintenance(this IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMiddleware<UnderMaintenanceHandler>();
        }
    }
}