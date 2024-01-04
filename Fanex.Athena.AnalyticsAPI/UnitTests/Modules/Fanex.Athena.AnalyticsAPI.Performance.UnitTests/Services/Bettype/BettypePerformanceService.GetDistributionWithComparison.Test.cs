using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    public class BettypePerformanceService_GetDistributionWithComparison_Test : BettypePerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var data = new List<IEnumerable<BettypeDistributionDTO>> { _fixture.CreateMany<BettypeDistributionDTO>(), _fixture.CreateMany<BettypeDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<BettypeDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetDistributionWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToDistributionVM(data[0], data[1], It.IsAny<IEnumerable<DistributionMetaModel>>()), Times.Once);
        }

        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<DistributionVM>();

            _mapperMock.Setup(_ => _.ToDistributionVM(It.IsAny<IEnumerable<BettypeDistributionDTO>>(), It.IsAny<IEnumerable<BettypeDistributionDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetDistributionWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}