using AutoFixture;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.UnitTest.Services
{
    public class HttpClientService_PostAsync_Test
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClientService _targetService;
        private readonly Fixture _fixture;

        public HttpClientService_PostAsync_Test()
        {
            _fixture = new Fixture();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("http://test.com");
            _targetService = new HttpClientService(httpClient);
        }

        [Fact]
        public async Task PostAsync_ErrorCode_ReturnNull()
        {
            //Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            //Act
            var result = await _targetService.PostAsync<object>(_fixture.Create<string>(), _fixture.Create<object>());
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PostAsync_SuccessCode_ReturnCorrectContent()
        {
            //Arrange
            var jsonContent = JsonConvert.SerializeObject(_fixture.Create<object>());

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonContent)
                });
            //Act
            var result = await _targetService.PostAsync<object>(_fixture.Create<string>(), _fixture.Create<object>());

            //Assert
            Assert.Equal(jsonContent, JsonConvert.SerializeObject(result));
        }
    }
}