using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    public class OverviewPerformanceService_GetDetailMonthGrowthWithComparison_Test : OverviewPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetDetailMonthGrowthWithComparison_WhenCall_GroupOfCriteriaIsMonthGroup()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var criteria = null as OverviewMeasurementGrowthCriteria;

            _repositoryMock.Setup(_ => _.QueryAsync<MeasurementTimeDTO>(It.IsAny<ICriteria>()))
                .Callback<ICriteria>(_criteria =>
                {
                    criteria = _criteria as OverviewMeasurementGrowthCriteria;
                });

            //Act
            await _target.GetDetailMonthGrowthWithComparison(filter);

            //Assert
            Assert.Equal(GroupDataConst.MONTH_GROUP, criteria.GroupID);
        }

        [Fact]
        public async Task GetDetailMonthGrowthWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var data = _fixture.CreateMany<MeasurementTimeDTO>();

            _repositoryMock.Setup(_ => _.QueryAsync<MeasurementTimeDTO>(It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            var fromMonth = DateTime.ParseExact(filter.CurrentPeriod.FromMonth, "MM/dd/yyyy", new CultureInfo("en-US"));
            var toMonth = DateTime.ParseExact(filter.CurrentPeriod.ToMonth, "MM/dd/yyyy", new CultureInfo("en-US"));

            //Act
            await _target.GetDetailMonthGrowthWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToMeasurementGrowthTimeVM(data, fromMonth, toMonth), Times.Once);
        }

        [Fact]
        public async Task GetDetailMonthGrowthWithComparison_WhenCall_ServiceResultIsMapperResult()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var expected = _fixture.CreateMany<MeasurementGrowthVM>();

            _mapperMock.Setup(_ => _.ToMeasurementGrowthTimeVM(It.IsAny<IEnumerable<MeasurementTimeDTO>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(expected);

            //Act
            var result = await _target.GetDetailMonthGrowthWithComparison(filter);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}