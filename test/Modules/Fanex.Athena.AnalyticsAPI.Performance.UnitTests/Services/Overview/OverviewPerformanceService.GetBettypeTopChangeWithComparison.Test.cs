namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using Data.Repository;
    using Moq;
    using Performance.Models.Criteria;
    using Performance.Models.DTO;
    using Performance.Models.ViewModels;
    using Xunit;

    public class OverviewPerformanceService_GetBettypeTopChangeWithComparison_Test : OverviewPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetBettypeTopChangeWithComparison_WhenCall_DimensionOfCriteriaIsCurrency()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var prevPeriodCriteria = null as OverviewTopChangeCriteria;
            var currentPeriodCriteria = null as OverviewTopChangeCriteria;

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<BettypeTopChangeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(listCriteria =>
                {
                    prevPeriodCriteria = listCriteria[0] as OverviewTopChangeCriteria;
                    currentPeriodCriteria = listCriteria[1] as OverviewTopChangeCriteria;
                });

            //Act
            await _target.GetBettypeTopChangeWithComparison(filter);

            //Assert
            Assert.Equal(DimensionConst.BETTYPE, prevPeriodCriteria.Dimension);
            Assert.Equal(DimensionConst.BETTYPE, currentPeriodCriteria.Dimension);
        }

        [Fact]
        public async Task GetBettypeTopChangeWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var data = new List<IEnumerable<BettypeTopChangeDTO>> { _fixture.CreateMany<BettypeTopChangeDTO>(), _fixture.CreateMany<BettypeTopChangeDTO>() };
            var prevPeriodDtos = null as IEnumerable<TopChangeDTO>;
            var currentPeriodDtos = null as IEnumerable<TopChangeDTO>;

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<BettypeTopChangeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            _mapperMock.Setup(_ => _.ToTopChangeVM(It.IsAny<IEnumerable<BettypeTopChangeDTO>>(), It.IsAny<IEnumerable<BettypeTopChangeDTO>>()))
                .Callback<IEnumerable<TopChangeDTO>, IEnumerable<TopChangeDTO>>((_prevPeriodDtos, _currentPeriodDtos) =>
                {
                    prevPeriodDtos = _prevPeriodDtos;
                    currentPeriodDtos = _currentPeriodDtos;
                });

            //Act
            await _target.GetBettypeTopChangeWithComparison(filter);

            //Assert
            Assert.True(prevPeriodDtos != null && currentPeriodDtos != null);
            Assert.True(prevPeriodDtos != data[0] && currentPeriodDtos != data[1]);
        }

        [Fact]
        public async Task GetBettypeTopChangeWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<TopChangeVM>();

            _mapperMock.Setup(_ => _.ToTopChangeVM(It.IsAny<IEnumerable<BettypeTopChangeDTO>>(), It.IsAny<IEnumerable<BettypeTopChangeDTO>>()))
                .Returns(expected);

            //Act
            var result = await _target.GetBettypeTopChangeWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}