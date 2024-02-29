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
    public class BettypePerformanceService_GetMeasurementDifferenceWithComparison_Test : BettypePerformanceServiceTestBase
    {
        [Fact]
        public async Task GetMeasurementDifferenceWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var data = new List<IEnumerable<BettypeMeasurementComparisonDTO>> { _fixture.CreateMany<BettypeMeasurementComparisonDTO>(), _fixture.CreateMany<BettypeMeasurementComparisonDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<BettypeMeasurementComparisonDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetMeasurementDifferenceWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToMeasurementComparisonVM(data[0], data[1], It.IsAny<IEnumerable<DistributionMetaModel>>()), Times.Once);
        }

        [Fact]
        public async Task GetMeasurementDifferenceWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementComparisonVM>();

            _mapperMock.Setup(_ => _.ToMeasurementComparisonVM(It.IsAny<IEnumerable<BettypeMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<BettypeMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetMeasurementDifferenceWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}