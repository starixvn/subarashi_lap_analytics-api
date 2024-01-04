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
    public class CurrencyPerformanceService_GetDistributionRawData_Test : CurrencyPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionRawData_WhenCall_ReturnCorrectData()
        {
            //Arrange
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            var expected = new List<IEnumerable<CurrencyDistributionDTO>> { _fixture.CreateMany<CurrencyDistributionDTO>(), _fixture.CreateMany<CurrencyDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<CurrencyDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(expected);

            //Act
            var (prevPeriodData, currentPeriodData) = await _target.GetDistributionRawData(filter);

            //Assert
            Assert.Equal(expected.ElementAtOrDefault(0), prevPeriodData);
            Assert.Equal(expected.ElementAtOrDefault(1), currentPeriodData);
        }
    }
}