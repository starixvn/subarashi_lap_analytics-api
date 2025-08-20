namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Data.Repository;
    using Moq;
    using Performance.Models.Criteria;
    using Performance.Models.DTO;
    using Performance.Models.ViewModels;
    using Xunit;

    public class ProductPerformanceService_GetOverallMeasurement_Test : ProductPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetOverallMeasurement_WhenCall_DrillDownTypeOfCriteriaIsNoneDrillDown()
        {
            //Arrange
            var filter = FakeProductPerformanceFilterMetaModel;
            var criteriaPrevPeriod = null as ProductDistributionCriteria;
            var criteriaCurrentPeriod = null as ProductDistributionCriteria;

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<TotalDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(listCriteria =>
                {
                    criteriaPrevPeriod = listCriteria[0] as ProductDistributionCriteria;
                    criteriaCurrentPeriod = listCriteria[1] as ProductDistributionCriteria;
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
            var filter = FakeProductPerformanceFilterMetaModel;
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
            var filter = FakeProductPerformanceFilterMetaModel;
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