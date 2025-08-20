namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Models.DataDomain;
    using Services;
    using Xunit;

    public class DefendencyInjectionExtensionTests
    {
        [Fact]
        public void AddDICore_RegistersAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpContextAccessor(); // Required by AuthInfoService

            // Act
            services.AddDICore();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verify IAuthInfoService is registered as transient
            var authInfoService1 = serviceProvider.GetService<IAuthInfoService>();
            var authInfoService2 = serviceProvider.GetService<IAuthInfoService>();
            Assert.NotNull(authInfoService1);
            Assert.NotNull(authInfoService2);
            Assert.NotSame(authInfoService1, authInfoService2); // Transient should create new instances

            // Verify IMetaModelBindingService is registered as singleton
            var bindingService1 = serviceProvider.GetService<IMetaModelBindingService>();
            var bindingService2 = serviceProvider.GetService<IMetaModelBindingService>();
            Assert.NotNull(bindingService1);
            Assert.NotNull(bindingService2);
            Assert.Same(bindingService1, bindingService2); // Singleton should return same instance
        }

        [Fact]
        public void AddDICore_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddDICore();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddDICore_CanBeCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpContextAccessor(); // Required by AuthInfoService

            // Act
            services.AddDICore();
            services.AddDICore(); // Should not throw

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var authInfoService = serviceProvider.GetService<IAuthInfoService>();
            Assert.NotNull(authInfoService);
        }

        [Fact]
        public void AddDICore_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpContextAccessor(); // Required by AuthInfoService

            // Act
            services.AddDICore();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            var authInfoService = serviceProvider.GetService<IAuthInfoService>();
            Assert.IsType<AuthInfoService>(authInfoService);

            var bindingService = serviceProvider.GetService<IMetaModelBindingService>();
            Assert.IsType<MetaModelBindingService>(bindingService);
        }

        [Fact]
        public void AddDICore_WithExistingServices_OverridesWithNewRegistration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpContextAccessor(); // Required by AuthInfoService
            var mockAuthInfoService = new MockAuthInfoService();
            services.AddSingleton<IAuthInfoService>(mockAuthInfoService);

            // Act
            services.AddDICore(); // This will add a new transient registration

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var authInfoService = serviceProvider.GetService<IAuthInfoService>();
            // The last registration wins, so it should be AuthInfoService, not the mock
            Assert.IsType<AuthInfoService>(authInfoService);
            Assert.NotSame(mockAuthInfoService, authInfoService);
        }

        [Fact]
        public void AddDICore_RegistersServicesWithCorrectLifetime()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDICore();

            // Assert
            var authInfoServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IAuthInfoService));
            Assert.NotNull(authInfoServiceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, authInfoServiceDescriptor.Lifetime);

            var bindingServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IMetaModelBindingService));
            Assert.NotNull(bindingServiceDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, bindingServiceDescriptor.Lifetime);
        }

        // Mock implementation for testing
        private class MockAuthInfoService : IAuthInfoService
        {
            public string SiteName => "MockSite";
            public string UserName => "MockUser";
            public int UserId => 123;
            public IEnumerable<CurrencyAgentInfo> CurrencyAgentList =>
                new List<CurrencyAgentInfo>();
            public IEnumerable<string> OfferedSubProductList => new List<string>();
            public string Language => "en-US";
            public int SpecialPlatform => 0;
            public int SpecialPlatformType => 0;
        }
    }
}
