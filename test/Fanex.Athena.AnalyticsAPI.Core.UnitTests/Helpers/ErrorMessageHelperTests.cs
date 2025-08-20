namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Helpers
{
    using System;
    using System.Net;
    using Core.Helpers;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class ErrorMessageHelperTests
    {
        [Fact]
        public void GenerateMessage_WithGenericException_ReturnsCorrectFormat()
        {
            // Arrange
            var context = CreateMockHttpContext();
            var exception = new Exception("Test Exception");

            // Act
            var message = ErrorMessageHelper.GenerateMessage(context, exception);

            // Assert
            Assert.Contains("ErrMsg: Test Exception", message);
            Assert.Contains("REQUEST INFO", message);
            Assert.Contains("Host: test.com", message);
            Assert.Contains("Path: /testpath", message);
            Assert.Contains("Remote IP: 127.0.0.1", message);
            Assert.Contains("EXCEPTION INFO", message);
            Assert.Contains("Message: Test Exception", message);
        }

        [Fact]
        public void GenerateMessage_WithInnerExceptions_IncludesAllExceptionInfo()
        {
            // Arrange
            var context = CreateMockHttpContext();
            var innerException2 = new Exception("Inner 2");
            var innerException1 = new Exception("Inner 1", innerException2);
            var outerException = new Exception("Outer", innerException1);

            // Act
            var message = ErrorMessageHelper.GenerateMessage(context, outerException);

            // Assert
            Assert.Contains("Message: Outer", message);
            Assert.Contains("Message: Inner 1", message);
            Assert.Contains("Message: Inner 2", message);
        }
        
        [Fact]
        public void GenerateMessage_WithNullRequest_HandlesGracefully()
        {
            // Arrange
            var context = new Mock<HttpContext>();
            context.Setup(c => c.Request).Returns((HttpRequest)null);
            var exception = new Exception("Test");

            // Act
            var message = ErrorMessageHelper.GenerateMessage(context.Object, exception);

            // Assert
            Assert.DoesNotContain("Host:", message);
            Assert.DoesNotContain("Path:", message);
        }

        private HttpContext CreateMockHttpContext()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Host).Returns(new HostString("test.com"));
            request.Setup(r => r.Path).Returns("/testpath");

            var connection = new Mock<ConnectionInfo>();
            connection.Setup(c => c.RemoteIpAddress).Returns(IPAddress.Parse("127.0.0.1"));

            var context = new Mock<HttpContext>();
            context.Setup(c => c.Request).Returns(request.Object);
            context.Setup(c => c.Connection).Returns(connection.Object);

            return context.Object;
        }
    }
} 