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
    public class OverviewPerformanceService_GetPeriodGrowthWithComparison_Test : OverviewPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_GroupOfCriteriaIsNoneGroup()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;

            var (prevOfPrevPeriodCriteria, prevPeriodCriteria, currentPeriodCriteria) = (null as OverviewMeasurementGrowthCriteria,
                null as OverviewMeasurementGrowthCriteria,
                null as OverviewMeasurementGrowthCriteria);

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<TotalDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(listCriteria =>
                {
                    prevOfPrevPeriodCriteria = listCriteria[0] as OverviewMeasurementGrowthCriteria;
                    prevPeriodCriteria = listCriteria[1] as OverviewMeasurementGrowthCriteria;
                    currentPeriodCriteria = listCriteria[2] as OverviewMeasurementGrowthCriteria;
                });

            //Act
            await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            Assert.Equal(GroupDataConst.NONE_GROUP, prevOfPrevPeriodCriteria.GroupID);
            Assert.Equal(GroupDataConst.NONE_GROUP, prevPeriodCriteria.GroupID);
            Assert.Equal(GroupDataConst.NONE_GROUP, currentPeriodCriteria.GroupID);
        }

        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var data = new List<IEnumerable<TotalDTO>> { _fixture.CreateMany<TotalDTO>(), _fixture.CreateMany<TotalDTO>(), _fixture.CreateMany<TotalDTO>() };

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<TotalDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act

            await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToMeasurementGrowthPeriodVM(data[0], data[1], data[2]), Times.Once);
        }

        [Fact]
        public async Task GetPeriodGrowthWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementGrowthVM>();

            _mapperMock.Setup(_ => _.ToMeasurementGrowthPeriodVM(It.IsAny<IEnumerable<TotalDTO>>(), It.IsAny<IEnumerable<TotalDTO>>(), It.IsAny<IEnumerable<TotalDTO>>()))
                .Returns(expected);

            //Act

            var result = await _target.GetPeriodGrowthWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}