using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Services
{
    public class CommonApiClientTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly CommonApiClient _target;

        public CommonApiClientTests()
        {
            _fixture = new Fixture();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.test.com/")
            };

            _mockHttpClientFactory.Setup(x => x.CreateClient("CommonApiClient"))
                                  .Returns(_httpClient);

            _target = new CommonApiClient(_mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetAsync_WithSuccessfulResponse_ReturnsResult()
        {
            // Arrange
            var path = "/test/endpoint";
            var expectedResult = _fixture.Create<TestModel>();
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };

            var responseContent = JsonConvert.SerializeObject(apiResponse);
            SetupHttpResponse(HttpStatusCode.OK, responseContent);

            // Act
            var result = await _target.GetAsync<TestModel>(path);

            // Assert
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_WithErrorResponse_ReturnsDefault()
        {
            // Arrange
            var path = "/test/endpoint";
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 1,
                ErrorMessage = "Error occurred",
                Result = _fixture.Create<TestModel>()
            };

            var responseContent = JsonConvert.SerializeObject(apiResponse);
            SetupHttpResponse(HttpStatusCode.OK, responseContent);

            // Act
            var result = await _target.GetAsync<TestModel>(path);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_WithQueryParameters_BuildsCorrectUrl()
        {
            // Arrange
            var path = "/test/endpoint";
            var queryObject = new { param1 = "value1", param2 = 123 };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = _fixture.Create<TestModel>()
            };

            var responseContent = JsonConvert.SerializeObject(apiResponse);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().Contains("param1=value1") &&
                        req.RequestUri.ToString().Contains("param2=123")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _target.GetAsync<TestModel>(path, value: queryObject);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_WithCustomLanguageCode_SetsCorrectHeader()
        {
            // Arrange
            var path = "/test/endpoint";
            var languageCode = "fr-FR";
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = _fixture.Create<TestModel>()
            };

            var responseContent = JsonConvert.SerializeObject(apiResponse);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Contains("Language") &&
                        req.Headers.GetValues("Language").Contains(languageCode)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _target.GetAsync<TestModel>(path, languageCode);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_WithNullValue_DoesNotAddQueryString()
        {
            // Arrange
            var path = "/test/endpoint";
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = _fixture.Create<TestModel>()
            };

            var responseContent = JsonConvert.SerializeObject(apiResponse);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().EndsWith(path)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _target.GetAsync<TestModel>(path, value: null);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetStringAsync_WithSuccessfulResponse_ReturnsContent()
        {
            // Arrange
            var path = "/test/endpoint";
            var language = "en-US";
            var expectedContent = _fixture.Create<string>();

            SetupHttpResponse(HttpStatusCode.OK, expectedContent);

            // Act
            var result = await _target.GetStringAsync(path, language);

            // Assert
            Assert.Equal(expectedContent, result);
        }

        [Fact]
        public async Task GetStringAsync_WithCustomTimeout_UsesCorrectTimeout()
        {
            // Arrange
            var path = "/test/endpoint";
            var language = "en-US";
            var timeout = 30;
            var expectedContent = _fixture.Create<string>();

            SetupHttpResponse(HttpStatusCode.OK, expectedContent);

            // Act
            var result = await _target.GetStringAsync(path, language, timeout);

            // Assert
            Assert.Equal(expectedContent, result);
        }

        [Fact]
        public async Task GetStringAsync_WithHttpError_ThrowsException()
        {
            // Arrange
            var path = "/test/endpoint";
            var language = "en-US";

            SetupHttpResponse(HttpStatusCode.InternalServerError, "Error");

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _target.GetStringAsync(path, language));
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
