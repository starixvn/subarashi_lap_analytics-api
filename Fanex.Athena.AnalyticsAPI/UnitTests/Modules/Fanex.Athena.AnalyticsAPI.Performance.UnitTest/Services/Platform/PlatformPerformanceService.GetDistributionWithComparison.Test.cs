using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class PlatformPerformanceService_GetDistributionWithComparison_Test : PlatformPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var data = new List<IEnumerable<PlatformDistributionDTO>> { _fixture.CreateMany<PlatformDistributionDTO>(), _fixture.CreateMany<PlatformDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<PlatformDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetDistributionWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToDistributionVM(data[0], data[1], It.IsAny<IEnumerable<ValueTuple<string, string>>>()), Times.Once);
        }

        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<DistributionVM>();

            _mapperMock.Setup(_ => _.ToDistributionVM(It.IsAny<IEnumerable<PlatformDistributionDTO>>(), It.IsAny<IEnumerable<PlatformDistributionDTO>>(), It.IsAny<IEnumerable<ValueTuple<string, string>>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetDistributionWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}