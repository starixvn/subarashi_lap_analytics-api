namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Data.Repository;
    using Moq;
    using Performance.Models.DTO;
    using Xunit;

    public class ProductPerformanceService_GetDistributionRawData_Test : ProductPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionRawData_WhenCall_ReturnCorrectData()
        {
            //Arrange
            var filter = FakeProductPerformanceFilterMetaModel;
            var expected = new List<IEnumerable<ProductDistributionDTO>> { _fixture.CreateMany<ProductDistributionDTO>(), _fixture.CreateMany<ProductDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<ProductDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(expected);

            //Act
            var (prevPeriodData, currentPeriodData) = await _target.GetDistributionRawData(filter);

            //Assert
            Assert.Equal(expected.ElementAtOrDefault(0), prevPeriodData);
            Assert.Equal(expected.ElementAtOrDefault(1), currentPeriodData);
        }
    }
}