using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class PlatformPerformanceService_GetMeasurementDifferenceWithComparison_Test : PlatformPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetMeasurementDifferenceWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var data = new List<IEnumerable<PlatformMeasurementComparisonDTO>> { _fixture.CreateMany<PlatformMeasurementComparisonDTO>(), _fixture.CreateMany<PlatformMeasurementComparisonDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<PlatformMeasurementComparisonDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
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
            var filter = FakePlatformPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementComparisonVM>();

            _mapperMock.Setup(_ => _.ToMeasurementComparisonVM(It.IsAny<IEnumerable<PlatformMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<PlatformMeasurementComparisonDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetMeasurementDifferenceWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}