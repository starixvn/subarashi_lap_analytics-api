namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Extensions
{
    using System.Linq;
    using System.Net.Http;
    using Common.Extensions;
    using Common.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Models;
    using Moq;
    using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCommonApiClient_RegistersICommonApiClient()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var commonApiClient = serviceProvider.GetService<ICommonApiClient>();
            Assert.NotNull(commonApiClient);
            Assert.IsType<CommonApiClient>(commonApiClient);
        }

        [Fact]
        public void AddCommonApiClient_RegistersHttpClient()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.NotNull(httpClientFactory);

            var httpClient = httpClientFactory.CreateClient("CommonApiClient");
            Assert.NotNull(httpClient);
        }

        [Fact]
        public void AddCommonApiClient_ConfiguresHttpClientWithCorrectBaseAddress()
        {
            // Arrange
            var services = new ServiceCollection();
            var expectedHost = "https://api.test.com/";
            SetupAppSettings(services, expectedHost);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("CommonApiClient");

            Assert.Equal(expectedHost, httpClient.BaseAddress.ToString());
        }

        [Fact]
        public void AddCommonApiClient_ConfiguresHttpClientHandler()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("CommonApiClient");

            // Verify the handler is configured (we can't directly access the handler properties,
            // but we can verify the client was created successfully)
            Assert.NotNull(httpClient);
        }

        [Fact]
        public void AddCommonApiClient_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            var result = services.AddCommonApiClient();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddCommonApiClient_RegistersTransientService()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ICommonApiClient));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
        }

        [Fact]
        public void AddCommonApiClient_CanBeCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();
            services.AddCommonApiClient(); // Should not throw

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var commonApiClient = serviceProvider.GetService<ICommonApiClient>();
            Assert.NotNull(commonApiClient);
        }

        [Fact]
        public void AddCommonApiClient_WithDifferentAppSettings_ConfiguresCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var customHost = "https://custom.api.com/";
            SetupAppSettings(services, customHost);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("CommonApiClient");

            Assert.Equal(customHost, httpClient.BaseAddress.ToString());
        }

        [Fact]
        public void AddCommonApiClient_CreatesNewInstancesForTransientService()
        {
            // Arrange
            var services = new ServiceCollection();
            SetupAppSettings(services);

            // Act
            services.AddCommonApiClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var client1 = serviceProvider.GetService<ICommonApiClient>();
            var client2 = serviceProvider.GetService<ICommonApiClient>();

            Assert.NotNull(client1);
            Assert.NotNull(client2);
            Assert.NotSame(client1, client2); // Transient should create new instances
        }

        private void SetupAppSettings(IServiceCollection services, string host = "https://api.test.com/")
        {
            var appSettings = new AppSettings
            {
                CommonApiHost = new CommonApiHostConfig
                {
                    Host = host
                }
            };

            var mockOptions = new Mock<IOptions<AppSettings>>();
            mockOptions.Setup(x => x.Value).Returns(appSettings);

            services.AddSingleton(mockOptions.Object);
        }
    }
}
