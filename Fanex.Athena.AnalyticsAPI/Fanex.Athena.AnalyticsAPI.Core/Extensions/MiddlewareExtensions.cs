namespace Fanex.Athena.AnalyticsAPI.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using Middleware;
    using Newtonsoft.Json;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;

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
            services.AddSingleton<IHttpContextHelper, ContextInMemoryHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
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

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping") || context.Request.Path.StartsWithSegments("/health"))
                {
                    return;
                }
                await next();
            });
        }

        public static IApplicationBuilder UseWhoAmICheck(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/whoami", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            status = report.Status.ToString(),
                            monitors = report.Entries.Select(e => new { name = e.Key, status = Enum.GetName(typeof(HealthStatus), e.Value.Status), values = e.Value.Data })
                        }, Formatting.Indented);

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
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
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = isDev ?
                    "no-cache" : "public,max-age=" + OneYear;
                    ctx.Context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                }
            });
        }

        public static void UseUnderMaintenance(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnderMaintenanceHandler>();
        }

        internal class ContextInMemoryHelper : IHttpContextHelper
        {
            private readonly TimeSpan ByPassUMCacheTime = TimeSpan.FromMinutes(30);
            private readonly IHttpContextAccessor httpContextAccessor;
            private readonly IMemoryCache cache;

            public ContextInMemoryHelper(IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
            {
                this.httpContextAccessor = httpContextAccessor;
                this.cache = cache;
            }

            public string GetSession(string key)
                => cache.TryGetValue(BuildKey(key), out string value) ? value : string.Empty;

            public string GetValueFromQueryString(string queryKey)
                => httpContextAccessor.HttpContext?.Request.Query[queryKey].ToString();

            public void SetSession(string key, string value)
                => cache.Set(BuildKey(key), value, ByPassUMCacheTime);

            private string BuildKey(string key)
                => $"{key}:{httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString()}";
        }
    }
}