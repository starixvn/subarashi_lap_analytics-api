using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class OverviewPerformanceService_GetProductTopChangeWithComparison_Test : OverviewPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetProductTopChangeWithComparison_WhenCall_DimensionOfCriteriaIsCurrency()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var prevPeriodCriteria = null as OverviewTopChangeCriteria;
            var currentPeriodCriteria = null as OverviewTopChangeCriteria;

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<ProductTopChangeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(listCriteria =>
                {
                    prevPeriodCriteria = listCriteria[0] as OverviewTopChangeCriteria;
                    currentPeriodCriteria = listCriteria[1] as OverviewTopChangeCriteria;
                });

            //Act
            await _target.GetProductTopChangeWithComparison(filter);

            //Assert
            Assert.Equal(DimensionConst.PRODUCT, prevPeriodCriteria.Dimension);
            Assert.Equal(DimensionConst.PRODUCT, currentPeriodCriteria.Dimension);
        }

        [Fact]
        public async Task GetProductTopChangeWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var data = new List<IEnumerable<ProductTopChangeDTO>> { _fixture.CreateMany<ProductTopChangeDTO>(), _fixture.CreateMany<ProductTopChangeDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<ProductTopChangeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetProductTopChangeWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToTopChangeVM(data[0], data[1]), Times.Once);
        }

        [Fact]
        public async Task GetProductTopChangeWithComparison_WhenCall_TranslateServiceCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<TopChangeVM>();

            _mapperMock.Setup(_ => _.ToTopChangeVM(It.IsAny<IEnumerable<ProductTopChangeDTO>>(), It.IsAny<IEnumerable<ProductTopChangeDTO>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetProductTopChangeWithComparison(filter);

            //Assert
            _translationServiceMock.Verify(_ => _.Translate(expected, filter.LangCode, "product"), Times.Once);
            Assert.Equal(expected, result);
        }
    }
}