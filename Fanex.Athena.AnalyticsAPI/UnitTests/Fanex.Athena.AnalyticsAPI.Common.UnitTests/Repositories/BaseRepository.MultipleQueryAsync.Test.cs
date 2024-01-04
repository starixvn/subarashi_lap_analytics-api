using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Repositories
{
    public class BaseRepository_MultipleQueryAsync_Test
    {
        private readonly Fixture _fixture;

        private readonly Mock<IOptions<SSASSetting>> _mockSetting;
        private readonly Mock<IDaxQueryContentService> _mockDaxQueryService;
        private readonly Mock<ISsasBridgeService> _mockBridgeService;

        private BaseRepositoryTestable _baseRepository;

        public BaseRepository_MultipleQueryAsync_Test()
        {
            _fixture = new Fixture();

            _mockSetting = new Mock<IOptions<SSASSetting>>();
            _mockDaxQueryService = new Mock<IDaxQueryContentService>();
            _mockBridgeService = new Mock<ISsasBridgeService>();
        }

        [Fact]
        public async Task QueryMultipleAsync_QueryContentParam_WhenCall_BridgeServiceCallSameParamsAndReturnCorrectResult()
        {
            //Arrange
            var queries = _fixture.CreateMany<string>();
            var expected = _fixture.CreateMany<IEnumerable<object>>();

            _mockBridgeService.Setup(_ => _.QueryMultipleAsync<object>(queries, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryMultipleAsync<object>(queries);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task QueryMultipleAsync_CriteriaParam_WhenCall_DaxQueryServiceCallWithCorrectCriteria()
        {
            //Arrange
            var listCriteria = new Mock<List<ICriteria>>().Object.ToArray();

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            await _baseRepository.QueryMultipleAsync<object>(listCriteria);

            //Assert
            _mockDaxQueryService.Verify(_ => _.GetMultipleDaxQuery(listCriteria), Times.Once);
        }

        [Fact]
        public async Task QueryMultipleAsync_CriteriaParam_WhenCall_BridgeServiceCallWithCorrectQueryAndReturnCorrectResult()
        {
            //Arrange
            var queries = _fixture.CreateMany<string>();
            var expected = _fixture.CreateMany<IEnumerable<object>>();
            var listCriteria = new Mock<List<ICriteria>>().Object.ToArray();

            _mockDaxQueryService.Setup(_ => _.GetMultipleDaxQuery(It.IsAny<IEnumerable<ICriteria>>())).Returns(() => Task.FromResult(queries));

            _mockBridgeService.Setup(_ => _.QueryMultipleAsync<object>(queries, It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected));

            _baseRepository = new BaseRepositoryTestable(_mockSetting.Object, _mockDaxQueryService.Object, _mockBridgeService.Object);

            //Act
            var actual = await _baseRepository.QueryMultipleAsync<object>(listCriteria);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}