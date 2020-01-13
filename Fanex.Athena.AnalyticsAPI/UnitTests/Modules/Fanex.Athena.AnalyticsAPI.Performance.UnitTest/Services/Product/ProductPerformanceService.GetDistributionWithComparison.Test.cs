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