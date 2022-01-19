using AutoFixture;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.UnitTests.Services
{
    public class SsasBridgeService_QueryMultipleAsync_Test
    {
        private readonly Mock<IHttpClientService> _mockHttClientService;
        private readonly SsasBridgeService _targetService;
        private readonly Fixture _fixture;

        public SsasBridgeService_QueryMultipleAsync_Test()
        {
            _fixture = new Fixture();
            _mockHttClientService = new Mock<IHttpClientService>();
            _targetService = new SsasBridgeService(_mockHttClientService.Object);
            _targetService.Setting = new Models.BridgeSetting() { ConnectionKey = _fixture.Create<string>() };
        }

        [Fact]
        public async Task QueryMultipleAsync_ResponseNull_ReturnEmptyResult()
        {
            //Arrange
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IEnumerable<IDictionary<string, object>>>>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(null as IEnumerable<IEnumerable<IDictionary<string, object>>>);

            //Act
            var result = await _targetService.QueryMultipleAsync<object>(_fixture.CreateMany<string>());

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task QueryMultipleAsync_WhenCall_HttpClientServiceCallWithCorrectParams()
        {
            //Arrange
            var url = null as string;
            var body = null as object;
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IEnumerable<IDictionary<string, object>>>>(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((_url, _body) =>
                {
                    url = _url;
                    body = _body;
                });
            var queries = _fixture.CreateMany<string>();
            //Act
            await _targetService.QueryMultipleAsync<object>(queries);

            //Assert
            Assert.Equal("SSAS/QueryMultiple", url);
            Assert.Equal(JsonConvert.SerializeObject(new { queries, connectionKey = _targetService.Setting.ConnectionKey }), JsonConvert.SerializeObject(body));
        }

        [Fact]
        public async Task QueryMultipleAsync_WhenCall_ReturnResultIsNotEmpty()
        {
            //Arrange
            var responseData = _fixture.CreateMany<IEnumerable<IDictionary<string, object>>>(10);
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IEnumerable<IDictionary<string, object>>>>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(responseData);

            //Act
            var result = await _targetService.QueryMultipleAsync<object>(_fixture.CreateMany<string>());

            //Assert
            Assert.Equal(responseData.Count(), result.Count());
        }
    }
}