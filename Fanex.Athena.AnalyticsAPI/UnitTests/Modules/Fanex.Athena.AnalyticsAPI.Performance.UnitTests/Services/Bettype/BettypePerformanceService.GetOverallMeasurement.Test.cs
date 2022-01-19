using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    public class BettypePerformanceService_GetOverallMeasurement_Test : BettypePerformanceServiceTestBase
    {
        [Fact]
        public async Task GetOverallMeasurement_WhenCall_DrillDownTypeOfCriteriaIsNoneDrillDown()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var criteriaPrevPeriod = null as BettypeDistributionCriteria;
            var criteriaCurrentPeriod = null as BettypeDistributionCriteria;

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<TotalDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(listCriteria =>
                {
                    criteriaPrevPeriod = listCriteria[0] as BettypeDistributionCriteria;
                    criteriaCurrentPeriod = listCriteria[1] as BettypeDistributionCriteria;
                });

            //Act
            await _target.GetOverallMeasurement(filter);

            //Assert
            Assert.Equal(DrillDownConst.NONE_DRILL_DOWN, criteriaPrevPeriod.DrillDown);
            Assert.Equal(DrillDownConst.NONE_DRILL_DOWN, criteriaCurrentPeriod.DrillDown);
        }

        [Fact]
        public async Task GetOverallMeasurement_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var data = new List<IEnumerable<TotalDTO>> { _fixture.CreateMany<TotalDTO>(), _fixture.CreateMany<TotalDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<TotalDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetOverallMeasurement(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToOverallMeasurementVM(data[0].FirstOrDefault(), data[1].FirstOrDefault()), Times.Once);
        }

        [Fact]
        public async Task GetOverallMeasurement_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakBettypePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<OverallMeasurementVM>();

            _mapperMock.Setup(_ => _.ToOverallMeasurementVM(It.IsAny<TotalDTO>(), It.IsAny<TotalDTO>()))
                .Returns(expected);

            //Act
            var result = await _target.GetOverallMeasurement(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}