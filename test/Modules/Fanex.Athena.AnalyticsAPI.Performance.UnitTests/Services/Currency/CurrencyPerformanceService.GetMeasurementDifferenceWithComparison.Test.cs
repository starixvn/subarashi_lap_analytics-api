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

    public class CurrencyPerformanceService_GetMeasurementDifferenceWithComparison_Test : CurrencyPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetMeasurementDifferenceWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            var data = new List<IEnumerable<CurrencyMeasurementComparisonDTO>> { _fixture.CreateMany<CurrencyMeasurementComparisonDTO>(), _fixture.CreateMany<CurrencyMeasurementComparisonDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<CurrencyMeasurementComparisonDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
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
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementComparisonVM>();

            _mapperMock.Setup(_ => _.ToMeasurementComparisonVM(It.IsAny<IEnumerable<CurrencyMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<CurrencyMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetMeasurementDifferenceWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}