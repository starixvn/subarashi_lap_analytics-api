namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using Data.Repository;
    using Moq;
    using Performance.Models.DTO;
    using Performance.Models.MetaModels;
    using Performance.Models.ViewModels;
    using Xunit;

    public class PlatformPerformanceService_GetPeriodGrowthWithComparison_Test : PlatformPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var data = new List<IEnumerable<PlatformMeasurementPeriodDTO>> { _fixture.CreateMany<PlatformMeasurementPeriodDTO>(), _fixture.CreateMany<PlatformMeasurementPeriodDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<PlatformMeasurementPeriodDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToMeasurementGrowthPeriodVM(data[0], data[1], It.IsAny<IEnumerable<DistributionMetaModel>>()), Times.Once);
        }

        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementPeriodComparisonGrowthVM>();

            _mapperMock.Setup(_ => _.ToMeasurementGrowthPeriodVM(It.IsAny<IEnumerable<PlatformMeasurementPeriodDTO>>(), It.IsAny<IEnumerable<PlatformMeasurementPeriodDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}