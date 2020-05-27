using Fanex.Athena.AnalyticsAPI.Core.Middleware;
using Fanex.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Core.UnitTest.Middleware
{
    public class ErrorHandlerTest
    {
        private readonly Mock<ILogger> logger;

        private const string ExpectedResponseBody = "Completed";

        private const string ContentType = "application/json";

        private const string ExceptionMessage = "ExceptionMessage";

        public ErrorHandlerTest()
        {
            logger = new Mock<ILogger>();
        }

        [Fact]
        public async Task Invoke_FeatureIsIExceptionHandlerFeature_ResponseHasCorrectContent()
        {
            // Arrange
            var middleware = CreateMiddleware(true);
            var context = CreateHttpContext();
            var expectedResponseContent = JsonConvert.SerializeObject(
                    new
                    {
                        state = "Internal Server Error",
                        msg = "Sorry, an error occurred while processing your request.",
                        isSuccess = false
                    });

            // Action
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var stream = new StreamReader(context.Response.Body))
            {
                var result = stream.ReadToEnd();

                // Assert
                Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
                Assert.Equal(ContentType, context.Response.ContentType);
                Assert.Equal(expectedResponseContent, result);
            }
        }

        [Fact]
        public async Task Invoke_FeatureIsOthers_ResponseHasCorrectContent()
        {
            // Arrange
            var middleware = CreateMiddleware();
            var context = CreateHttpContext(hasFeature: false);

            // Action
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var stream = new StreamReader(context.Response.Body))
            {
                var result = stream.ReadToEnd();

                // Assert
                Assert.Equal(ExpectedResponseBody, result);
            }
        }

        [Fact]
        public async Task Invoke_ThrowExceptionWhenCallingNext_ErrorIsLoggedAndResponseHasCorrectContent()
        {
            // Arrange
            var middleware = CreateMiddleware(throwException: true);
            var context = CreateHttpContext(hasFeature: false);
            var expectedResponseContent = JsonConvert.SerializeObject(new
            {
                state = "Internal Server Error",
                msg = "Sorry, an error occurred while processing your request.",
                isSuccess = false
            });

            // Action
            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var stream = new StreamReader(context.Response.Body))
            {
                var result = stream.ReadToEnd();

                // Assert
                Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
                Assert.Equal(ContentType, context.Response.ContentType);
                Assert.Equal(expectedResponseContent, result);
                logger.Verify(r => r.ErrorAsync(It.IsAny<string>()), Times.Once);
            }
        }

        private ErrorHandler CreateMiddleware(bool throwException = false)
        {
            return new ErrorHandler(
                next: (innerHttpContext) =>
                {
                    if (throwException)
                    {
                        throw new Exception(ExceptionMessage);
                    }
                    else
                    {
                        innerHttpContext.Response.WriteAsync(ExpectedResponseBody);
                        return Task.CompletedTask;
                    }
                },
                logger: logger.Object
            );
        }

        private HttpContext CreateHttpContext(bool hasFeature = true)
        {
            var response = Mock.Of<HttpResponse>(r => r.Body == new MemoryStream());
            var context = Mock.Of<HttpContext>(ctx => ctx.Response == response && ctx.Features == Mock.Of<IFeatureCollection>());

            if (hasFeature)
            {
                var exception = Mock.Of<IExceptionHandlerFeature>(e => e.Error == new Exception(ExceptionMessage));
                var features = Mock.Of<IFeatureCollection>(f => f[typeof(IExceptionHandlerFeature)] == exception);
                Mock.Get(context).Setup(x => x.Features).Returns(features);
            }

            return context;
        }
    }
}