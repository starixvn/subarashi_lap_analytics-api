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

    public class ProductPerformanceService_GetDistributionWithComparison_Test : ProductPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakeProductPerformanceFilterMetaModel;
            var data = new List<IEnumerable<ProductDistributionDTO>> { _fixture.CreateMany<ProductDistributionDTO>(), _fixture.CreateMany<ProductDistributionDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<ProductDistributionDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetDistributionWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToDistributionVM(data[0], data[1], It.IsAny<IEnumerable<DistributionMetaModel>>()), Times.Once);
        }

        [Fact]
        public async Task GetDistributionWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakeProductPerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<DistributionVM>();

            _mapperMock.Setup(_ => _.ToDistributionVM(It.IsAny<IEnumerable<ProductDistributionDTO>>(), It.IsAny<IEnumerable<ProductDistributionDTO>>(), It.IsAny<IEnumerable<DistributionMetaModel>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetDistributionWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}