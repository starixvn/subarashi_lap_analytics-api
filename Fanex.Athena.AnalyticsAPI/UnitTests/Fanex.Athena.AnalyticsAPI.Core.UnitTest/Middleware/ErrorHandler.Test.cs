using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Fanex.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.Middleware
{
    public class ErrorHandler_Test : IDisposable
    {
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<IExceptionHandlerFeature> _mockHandlerException;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<HttpResponse> _mockResponse;
        private readonly Mock<ILogger> _mockLogger;

        private Stream BodyContent;

        public ErrorHandler_Test()
        {
            LogManager
            .SetDefaultLogCategory("TEST")
            .Use(new List<ILogEngine>
             {
                new Mock<ILogEngine>().Object
             });
            _mockHttpContext = new Mock<HttpContext>();
            _mockResponse = new Mock<HttpResponse>();
            _mockRequest = new Mock<HttpRequest>();
            _mockLogger = new Mock<ILogger>();
            _mockHttpContext.Setup(x => x.Response).Returns(_mockResponse.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockHandlerException = new Mock<IExceptionHandlerFeature>();
        }

        [Fact]
        public async Task Invoke_RequestError_ReturnErrorMessageAsync()
        {
            //Arrange
            var actualContent = string.Empty;
            var actualStatus = 0;
            var actualContentType = string.Empty;
            _mockHandlerException.Setup(x => x.Error).Returns(new Exception());
            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(_mockHandlerException.Object);

            var bodyJson = JsonConvert.SerializeObject(new { A = "123", B = "asdsad" });
            BodyContent = new MemoryStream(Encoding.UTF8.GetBytes(bodyJson));
            _mockRequest.Setup(_ => _.Body).Returns(BodyContent);

            _mockResponse.Setup(x => x.Body.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .Callback<byte[], int, int, CancellationToken>((data, offset, length, cancel) =>
               {
                   if (length > 0)
                       actualContent = Encoding.UTF8.GetString(data);
               });
            _mockResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(x => { actualStatus = x; });
            _mockResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(x => { actualContentType = x; });

            var expectedResult = JsonConvert.SerializeObject(new
            {
                state = "Internal Server Error",
                msg = "Sorry, an error occurred while processing your request.",
                isSuccess = false
            });

            var errorHandler = new ErrorHandler(next: async (x) =>
            {
                await Task.CompletedTask;
            }, logger: _mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert

            Assert.Equal(500, actualStatus);
            Assert.Equal("application/json", actualContentType);
            Assert.Equal(expectedResult, actualContent);
        }

        [Fact]
        public async Task Invoke_Exception_ReturnErrorMessageAsync()
        {
            //Arrange
            var actualContent = string.Empty;
            var actualStatus = 0;
            var actualContentType = string.Empty;

            _mockHttpContext.Setup(x => x.Features[typeof(IExceptionHandlerFeature)])
                .Returns(new List<IExceptionHandlerFeature>());

            var bodyJson = JsonConvert.SerializeObject(new { A = "123", B = "asdsad" });
            BodyContent = new MemoryStream(Encoding.UTF8.GetBytes(bodyJson));
            _mockRequest.Setup(_ => _.Body).Returns(BodyContent);

            _mockResponse.Setup(x => x.Body.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .Callback<byte[], int, int, CancellationToken>((data, offset, length, cancel) =>
               {
                   if (length > 0)
                       actualContent = Encoding.UTF8.GetString(data);
               });

            _mockResponse.SetupSet(x => x.StatusCode = It.IsAny<int>()).Callback<int>(x => { actualStatus = x; });
            _mockResponse.SetupSet(x => x.ContentType = It.IsAny<string>()).Callback<string>(x => { actualContentType = x; });

            var expectedResult = JsonConvert.SerializeObject(new
            {
                state = "Internal Server Error",
                msg = "Sorry, an error occurred while processing your request.",
                isSuccess = false
            });

            var errorHandler = new ErrorHandler(next: async (x) =>
            {
                await Task.CompletedTask;
                throw new Exception();
            }, logger: _mockLogger.Object);

            //Action
            await errorHandler.Invoke(_mockHttpContext.Object).ConfigureAwait(false);

            //Assert

            Assert.Equal(500, actualStatus);
            Assert.Equal("application/json", actualContentType);
            Assert.Equal(expectedResult, actualContent);
        }

        public void Dispose()
        {
            BodyContent?.Dispose();
        }
    }
}