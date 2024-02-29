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
    public class ProductPerformanceService_GetPeriodGrowthWithComparison_Test : ProductPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakeProductPerformanceFilterMetaModel;
            var data = new List<IEnumerable<ProductMeasurementPeriodDTO>> { _fixture.CreateMany<ProductMeasurementPeriodDTO>(), _fixture.CreateMany<ProductMeasurementPeriodDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<ProductMeasurementPeriodDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
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
            var filter = FakeProductPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementPeriodComparisonGrowthVM>();

            _mapperMock.Setup(_ => _.ToMeasurementGrowthPeriodVM(It.IsAny<IEnumerable<ProductMeasurementPeriodDTO>>(), It.IsAny<IEnumerable<ProductMeasurementPeriodDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}