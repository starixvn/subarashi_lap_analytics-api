namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Data.Repository;
    using Moq;
    using Performance.Models.Criteria;
    using Performance.Models.DTO;
    using Performance.Models.MetaModels;
    using Performance.Models.ViewModels;
    using Xunit;

    public class CurrencyPerformanceService_GetAllDetailMonthGrowthWithComparison_Test : CurrencyPerformanceServiceTestBase
    {
        [Fact]
        public async Task GetAllDetailMonthGrowthWithComparison_WhenCall_CorrectNumberOfCriteria_GroupByIsMonthGroup()
        {
            //Arrange
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            var cloneFilter = filter;
            cloneFilter.TopDistributions = _fixture.CreateMany<DistributionMetaModel>();

            var listCriteria = new List<CurrencyGrowthCriteria>(cloneFilter.TopDistributions.Count() * 2);

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<MeasurementTimeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .Callback<ICriteria[]>(_listCriteria =>
                {
                    listCriteria.AddRange(_listCriteria.Select(_ => _ as CurrencyGrowthCriteria));
                });

            //Act
            await _target.GetAllDetailMonthGrowthWithComparison(filter);

            //Assert
            Assert.Equal(cloneFilter.TopDistributions.Count() * 2, listCriteria.Count);
            Assert.True(listCriteria.All(_ => _.GroupBy == GroupByConst.MONTH_GROUP));
        }

        [Fact]
        public async Task GetAllDetailMonthGrowthWithComparison_WhenCall_MapperShouldCallCorrectParams()
        {
            //Arrange
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            filter.TopDistributions = _fixture.CreateMany<DistributionMetaModel>();

            var data = new List<IEnumerable<MeasurementTimeDTO>> { _fixture.CreateMany<MeasurementTimeDTO>(), _fixture.CreateMany<MeasurementTimeDTO>() };

            var (prevPeriodFromDate, prevPeriodToDate) = (
                 DateTime.ParseExact(filter.PreviousPeriod.FromMonth, "MM/dd/yyyy", new CultureInfo("en-US")),
                 DateTime.ParseExact(filter.PreviousPeriod.ToMonth, "MM/dd/yyyy", new CultureInfo("en-US"))
                );

            var (currentPeriodFromDate, currentPeriodToDate) = (
                DateTime.ParseExact(filter.CurrentPeriod.FromMonth, "MM/dd/yyyy", new CultureInfo("en-US")),
                DateTime.ParseExact(filter.CurrentPeriod.ToMonth, "MM/dd/yyyy", new CultureInfo("en-US"))
                );

            _repositoryMock.Setup(_ => _.QueryMultipleAsync<MeasurementTimeDTO>(It.IsAny<ICriteria>(), It.IsAny<ICriteria>()))
                .ReturnsAsync(data);

            //Act
            await _target.GetAllDetailMonthGrowthWithComparison(filter);

            //Assert
            _mapperMock.Verify(_ => _.ToMeasurementGrowthTimeVM(data.ElementAt(0), prevPeriodFromDate, prevPeriodToDate), Times.Exactly(filter.TopDistributions.Count()));
            _mapperMock.Verify(_ => _.ToMeasurementGrowthTimeVM(data.ElementAt(1), currentPeriodFromDate, currentPeriodToDate), Times.Exactly(filter.TopDistributions.Count()));
        }

        [Fact]
        public async Task GetAllDetailMonthGrowthWithComparison_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var filter = FakeCurrencyPerformanceFilterMetaModel;
            filter.TopDistributions = _fixture.CreateMany<DistributionMetaModel>();

            var dataVM = new List<IEnumerable<MeasurementGrowthVM>> { _fixture.CreateMany<MeasurementGrowthVM>(), _fixture.CreateMany<MeasurementGrowthVM>() };

            var (prevPeriodFromDate, prevPeriodToDate) = (
                 DateTime.ParseExact(filter.PreviousPeriod.FromMonth, "MM/dd/yyyy", new CultureInfo("en-US")),
                 DateTime.ParseExact(filter.PreviousPeriod.ToMonth, "MM/dd/yyyy", new CultureInfo("en-US"))
                );

            var (currentPeriodFromDate, currentPeriodToDate) = (
                DateTime.ParseExact(filter.CurrentPeriod.FromMonth, "MM/dd/yyyy", new CultureInfo("en-US")),
                DateTime.ParseExact(filter.CurrentPeriod.ToMonth, "MM/dd/yyyy", new CultureInfo("en-US"))
                );

            _mapperMock.Setup(_ => _.ToMeasurementGrowthTimeVM(It.IsAny<IEnumerable<MeasurementTimeDTO>>(), prevPeriodFromDate, prevPeriodToDate))
                .Returns(dataVM[0]);

            _mapperMock.Setup(_ => _.ToMeasurementGrowthTimeVM(It.IsAny<IEnumerable<MeasurementTimeDTO>>(), currentPeriodFromDate, currentPeriodToDate))
                .Returns(dataVM[1]);

            var prevPeriodSuffixFieldName = $"{filter.PreviousPeriod.FromMonth}_{filter.PreviousPeriod.ToMonth}";
            var currentPeriodSuffixFieldName = $"{filter.CurrentPeriod.FromMonth}_{filter.CurrentPeriod.ToMonth}";

            //Act
            var result = await _target.GetAllDetailMonthGrowthWithComparison(filter);

            //Assert

            var prevPeriodResults = result.Where(_ => _.Key.Contains(prevPeriodSuffixFieldName));
            Assert.Equal(filter.TopDistributions.Count(), prevPeriodResults.Count());
            Assert.True(prevPeriodResults.All(_ => _.Value.Equals(dataVM[0])));

            var currentPeriodResults = result.Where(_ => _.Key.Contains(currentPeriodSuffixFieldName));
            Assert.Equal(filter.TopDistributions.Count(), currentPeriodResults.Count());
            Assert.True(currentPeriodResults.All(_ => _.Value.Equals(dataVM[1])));
        }
    }
}