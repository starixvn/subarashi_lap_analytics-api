using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTest.Repositories
{
    public class BaseRepository_QueryAsync_Test
    {
        private readonly Fixture _fixture;

        private readonly Mock<IOptions<SSASSetting>> _mockSetting;
        private readonly Mock<IDaxQueryContentService> _mockDaxQueryService;
        private readonly Mock<ISsasBridgeService> _mockBridgeService;

        private BaseRepositoryTestable _baseRepository;

        public BaseRepository_QueryAsync_Test()
        {
            _fixture = new Fixture();

            _mockSetting = new Mock<IOptions<SSASSetting>>();
            _mockDaxQueryService = new Mock<IDaxQueryContentService>();
            _mockBridgeService = new Mock<ISsasBridgeService>();
        }

        [Fact]
        public async Task QueryAsync_QueryContentParam_WhenCall_BridgeServiceCallSameParams()
        {
            //Arrange
            var query = _fixture.Create<string>();
            var expected = _fixture.CreateMany<object>();

            _mockBridgeService.Setup(_ => _.QueryAsync<object>(query, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryAsync<object>(query);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryAsync_CriteriaParam_WhenCall_DaxQueryServiceCallWithCorrectCriteria()
        {
            //Arrange
            var criteria = new Mock<ICriteria>().Object;

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            await _baseRepository.QueryAsync<object>(criteria);

            //Assert
            _mockDaxQueryService.Verify(_ => _.GetDaxQuery(criteria), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_CriteriaParam_WhenCall_BridgeServiceCallWithCorrectQuery()
        {
            //Arrange
            var query = _fixture.Create<string>();
            var expected = _fixture.CreateMany<object>();

            _mockDaxQueryService.Setup(_ => _.GetDaxQuery(It.IsAny<ICriteria>())).Returns(() => Task.FromResult(query));

            _mockBridgeService.Setup(_ => _.QueryAsync<object>(query, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryAsync<object>(new Mock<ICriteria>().Object);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}