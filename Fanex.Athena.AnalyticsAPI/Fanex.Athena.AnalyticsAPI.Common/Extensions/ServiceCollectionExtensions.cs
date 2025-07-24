using System;
using System.Net;
using System.Net.Http;
using Fanex.Athena.AnalyticsAPI.Common.Models;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonApiClient(this IServiceCollection services)
        {
            const string CommonApiClient = "CommonApiClient";

            services.AddTransient<ICommonApiClient, CommonApiClient>();

            services.AddHttpClient(CommonApiClient, (serviceProvider, client) =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopeServiceProvider = scope.ServiceProvider;
                    var options = scopeServiceProvider.GetService<IOptions<AppSettings>>();

                    var setting = options.Value;
                    client.BaseAddress = new Uri(setting.CommonApiHost.Host);
                }
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });

            return services;
        }
    }
}