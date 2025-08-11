using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using ONEbook.UM.Services;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Middleware
{
    public class UnderMaintenanceHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<IMaintenanceService> _mockMaintenanceService;
        private readonly UnderMaintenanceHandler _target;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpResponse> _mockHttpResponse;

        public UnderMaintenanceHandlerTests()
        {
            _fixture = new Fixture();
            _mockNext = new Mock<RequestDelegate>();
            _mockMaintenanceService = new Mock<IMaintenanceService>();
            _target = new UnderMaintenanceHandler(_mockNext.Object, _mockMaintenanceService.Object);
            
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpContext.Setup(x => x.Response).Returns(_mockHttpResponse.Object);
        }

        [Fact]
        public async Task Invoke_WhenNotUnderMaintenance_CallsNextDelegate()
        {
            // Arrange
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(false);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(_mockHttpContext.Object), Times.Once);
            _mockHttpResponse.VerifySet(x => x.StatusCode = It.IsAny<int>(), Times.Never);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_ReturnsServiceUnavailable()
        {
            // Arrange
            var maintenanceMessage = _fixture.Create<string>();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(true);
            _mockMaintenanceService.Setup(x => x.GetMaintenanceMessage())
                                  .Returns(maintenanceMessage);

            var responseStream = new MemoryStream();
            _mockHttpResponse.Setup(x => x.Body).Returns(responseStream);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(_mockHttpContext.Object), Times.Never);
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.ServiceUnavailable, Times.Once);
            _mockHttpResponse.VerifySet(x => x.ContentType = "application/json", Times.Once);
            
            // Verify the response body contains the maintenance message
            responseStream.Position = 0;
            using var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            Assert.Contains(maintenanceMessage, responseContent);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceWithNullMessage_ReturnsDefaultMessage()
        {
            // Arrange
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(true);
            _mockMaintenanceService.Setup(x => x.GetMaintenanceMessage())
                                  .Returns((string)null);

            var responseStream = new MemoryStream();
            _mockHttpResponse.Setup(x => x.Body).Returns(responseStream);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.ServiceUnavailable, Times.Once);
            _mockHttpResponse.VerifySet(x => x.ContentType = "application/json", Times.Once);
            
            // Verify the response body contains a default message
            responseStream.Position = 0;
            using var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            Assert.Contains("under maintenance", responseContent.ToLowerInvariant());
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceWithEmptyMessage_ReturnsDefaultMessage()
        {
            // Arrange
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(true);
            _mockMaintenanceService.Setup(x => x.GetMaintenanceMessage())
                                  .Returns(string.Empty);

            var responseStream = new MemoryStream();
            _mockHttpResponse.Setup(x => x.Body).Returns(responseStream);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.ServiceUnavailable, Times.Once);
            _mockHttpResponse.VerifySet(x => x.ContentType = "application/json", Times.Once);
            
            // Verify the response body contains a default message
            responseStream.Position = 0;
            using var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            Assert.Contains("under maintenance", responseContent.ToLowerInvariant());
        }

        [Fact]
        public async Task Invoke_WhenMaintenanceServiceThrows_CallsNextDelegate()
        {
            // Arrange
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Throws(new Exception("Service error"));

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(_mockHttpContext.Object), Times.Once);
            _mockHttpResponse.VerifySet(x => x.StatusCode = It.IsAny<int>(), Times.Never);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_ReturnsJsonResponse()
        {
            // Arrange
            var maintenanceMessage = "System is under maintenance";
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(true);
            _mockMaintenanceService.Setup(x => x.GetMaintenanceMessage())
                                  .Returns(maintenanceMessage);

            var responseStream = new MemoryStream();
            _mockHttpResponse.Setup(x => x.Body).Returns(responseStream);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.ContentType = "application/json", Times.Once);
            
            responseStream.Position = 0;
            using var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            
            // Verify it's valid JSON structure
            Assert.StartsWith("{", responseContent.Trim());
            Assert.EndsWith("}", responseContent.Trim());
            Assert.Contains("\"message\":", responseContent);
            Assert.Contains(maintenanceMessage, responseContent);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenance_DoesNotCallNext()
        {
            // Arrange
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenance())
                                  .Returns(true);
            _mockMaintenanceService.Setup(x => x.GetMaintenanceMessage())
                                  .Returns("Maintenance");

            var responseStream = new MemoryStream();
            _mockHttpResponse.Setup(x => x.Body).Returns(responseStream);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(It.IsAny<HttpContext>()), Times.Never);
        }
    }
}
