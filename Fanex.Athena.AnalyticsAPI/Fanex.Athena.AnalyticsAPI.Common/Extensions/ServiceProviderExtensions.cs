using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class ServiceProviderExtensions
    {
        private static ServiceProvider _serviceProvider { get; set; }

        public static void SetServiceProvider(this ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static TService GetRequiredService<TService>()
        {
            var provider = _serviceProvider ?? throw new Exception($"{nameof(ServiceProvider)} is null");
            return provider.GetRequiredService<TService>();
        }
    }
}