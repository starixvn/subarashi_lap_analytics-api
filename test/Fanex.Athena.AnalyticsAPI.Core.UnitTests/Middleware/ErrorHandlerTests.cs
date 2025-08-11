using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTests.Middleware
{
    public class ErrorHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ErrorHandler>> _mockLogger;
        private readonly ErrorHandler _target;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpResponse> _mockHttpResponse;

        public ErrorHandlerTests()
        {
            _fixture = new Fixture();
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ErrorHandler>>();
            _target = new ErrorHandler(_mockNext.Object, _mockLogger.Object);
            
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpContext.Setup(x => x.Response).Returns(_mockHttpResponse.Object);
        }

        [Fact]
        public async Task Invoke_WhenNextDelegateSucceeds_DoesNotHandleError()
        {
            // Arrange
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .Returns(Task.CompletedTask);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(_mockHttpContext.Object), Times.Once);
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task Invoke_WhenNextDelegateThrowsException_HandlesError()
        {
            // Arrange
            var exception = new Exception(_fixture.Create<string>());
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockNext.Verify(x => x.Invoke(_mockHttpContext.Object), Times.Once);
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.InternalServerError, Times.Once);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenArgumentNullExceptionThrown_HandlesErrorCorrectly()
        {
            // Arrange
            var exception = new ArgumentNullException("paramName", "Parameter cannot be null");
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.InternalServerError, Times.Once);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenInvalidOperationExceptionThrown_HandlesErrorCorrectly()
        {
            // Arrange
            var exception = new InvalidOperationException("Invalid operation");
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.InternalServerError, Times.Once);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenExceptionWithInnerExceptionThrown_HandlesErrorCorrectly()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner exception");
            var exception = new Exception("Outer exception", innerException);
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.InternalServerError, Times.Once);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenTaskCanceledExceptionThrown_HandlesErrorCorrectly()
        {
            // Arrange
            var exception = new TaskCanceledException("Task was canceled");
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockHttpResponse.VerifySet(x => x.StatusCode = (int)HttpStatusCode.InternalServerError, Times.Once);
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_VerifyLoggerCalledWithCorrectMessage()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockNext.Setup(x => x.Invoke(_mockHttpContext.Object))
                    .ThrowsAsync(exception);

            // Act
            await _target.Invoke(_mockHttpContext.Object);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Test exception")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
