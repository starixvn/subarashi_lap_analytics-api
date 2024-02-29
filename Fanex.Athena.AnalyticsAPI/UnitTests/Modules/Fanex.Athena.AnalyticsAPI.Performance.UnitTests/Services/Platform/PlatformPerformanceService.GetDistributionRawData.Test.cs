using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    public class PlatformPerformanceService_GetDistributionRawData_Test : PlatformPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionRawData_WhenCall_ReturnCorrectData()
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            var expected = new List<IEnumerable<PlatformDistributionDTO>> { _fixture.CreateMany<PlatformDistributionDTO>(), _fixture.CreateMany<PlatformDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<PlatformDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(expected);

            //Act
            var (prevPeriodData, currentPeriodData) = await _target.GetDistributionRawData(filter);

            //Assert
            Assert.Equal(expected.ElementAtOrDefault(0), prevPeriodData);
            Assert.Equal(expected.ElementAtOrDefault(1), currentPeriodData);
        }
    }
}