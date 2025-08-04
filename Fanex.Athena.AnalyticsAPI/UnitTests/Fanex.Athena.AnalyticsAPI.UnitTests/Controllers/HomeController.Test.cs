using Fanex.Athena.AnalyticsAPI.Configuration;
using Fanex.Athena.AnalyticsAPI.Controllers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.UnitTests.Controllers
{
    public class HomeController_Test
    {
        private readonly HomeController _target;
        private readonly Mock<IOptions<StaticResourceConfig>> _mockOptions;
        private readonly StaticResourceConfig _staticResourceConfig;

        public HomeController_Test()
        {
            _staticResourceConfig = new StaticResourceConfig
            {
                // Add any properties that StaticResourceConfig might have
            };
            
            _mockOptions = new Mock<IOptions<StaticResourceConfig>>();
            _mockOptions.Setup(x => x.Value).Returns(_staticResourceConfig);
            
            _target = new HomeController();
        }

        [Fact]
        public void Static_WhenCalled_ReturnsStaticResourceConfig()
        {
            // Act
            var result = _target.Static(_mockOptions.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_staticResourceConfig, result);
        }

        [Fact]
        public void Static_WhenCalledWithNullConfig_ReturnsNull()
        {
            // Arrange
            var mockOptionsWithNull = new Mock<IOptions<StaticResourceConfig>>();
            mockOptionsWithNull.Setup(x => x.Value).Returns((StaticResourceConfig)null);

            // Act
            var result = _target.Static(mockOptionsWithNull.Object);

            // Assert
            Assert.Null(result);
        }
    }
}
