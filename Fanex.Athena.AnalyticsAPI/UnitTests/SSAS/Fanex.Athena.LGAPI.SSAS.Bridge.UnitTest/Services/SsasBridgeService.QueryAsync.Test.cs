using AutoFixture;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.UnitTest.Services
{
    public class SsasBridgeService_QueryAsync_Test
    {
        private readonly Mock<IHttpClientService> _mockHttClientService;
        private readonly SsasBridgeService _targetService;
        private readonly Fixture _fixture;

        public SsasBridgeService_QueryAsync_Test()
        {
            _fixture = new Fixture();
            _mockHttClientService = new Mock<IHttpClientService>();
            _targetService = new SsasBridgeService(_mockHttClientService.Object);
            _targetService.Setting = new Models.BridgeSetting();
        }

        [Fact]
        public async Task QueryAsync_ResponseNull_ReturnEmptyResult()
        {
            //Arrange
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IDictionary<string, object>>>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(null as IEnumerable<IDictionary<string, object>>);

            //Act
            var result = await _targetService.QueryAsync<object>(_fixture.Create<string>());

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task QueryAsync_WhenCall_HttpClientServiceCallWithCorrectParams()
        {

            //Arrange
            var url = null as string;
            var body = null as object;
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IDictionary<string, object>>>(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((_url, _body) =>
                {
                    url = _url;
                    body = _body;
                });
            var query = _fixture.Create<string>();
            //Act
            await _targetService.QueryAsync<object>(query);

            //Assert
            Assert.Equal("SSAS/Query", url);
            Assert.Equal(JsonConvert.SerializeObject(new { query, connectionKey = _targetService.Setting.ConnectionKey }), JsonConvert.SerializeObject(body));
        }

        [Fact]
        public async Task QueryAsync_WhenCall_ReturnResultIsNotEmpty()
        {
            //Arrange
            var responseData = _fixture.CreateMany<IDictionary<string, object>>(10);
            _mockHttClientService.Setup(_ => _.PostAsync<IEnumerable<IDictionary<string, object>>>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(responseData);

            //Act
            var result = await _targetService.QueryAsync<object>(_fixture.Create<string>());

            //Assert
            Assert.Equal(responseData.Count(), result.Count());
        }
    }
}