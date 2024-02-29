using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Controllers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Controllers
{
    public class BettypePerformanceController_Test
    {
        private readonly BettypePerformanceController _target;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<IBettypePerformanceService> _bettypeServiceMock;
        private readonly Mock<ITranslationService> _translationServiceMock;

        private readonly Fixture _fixture;

        public BettypePerformanceController_Test()
        {
            _contextServiceMock = new Mock<IContextService>();
            _bettypeServiceMock = new Mock<IBettypePerformanceService>();
            _translationServiceMock = new Mock<ITranslationService>();
            _fixture = new Fixture();
            _target = new BettypePerformanceController(_contextServiceMock.Object, _bettypeServiceMock.Object, _translationServiceMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetCombineData_WhenCall_ReturnCorrectResult(int measurementId)
        {
            //Arrange
            var filter = FakeBettypePerformanceFilterMetaModel;
            filter.Measurement.Id = measurementId;

            var topDistributionsData = _fixture.CreateMany<(string topDistributionId, string topDistributionName)>();
            var topDistributions = topDistributionsData.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName,
                CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } },
                PreviousPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } }
            }).ToList();

            _bettypeServiceMock.Setup(_ => _.GetTopDistributions(filter, 10)).ReturnsAsync(topDistributionsData);

            var overallMarginData = _fixture.CreateMany<OverallMeasurementVM>();
            _bettypeServiceMock.Setup(_ => _.GetOverallMeasurement(filter)).ReturnsAsync(overallMarginData);

            var distributionsData = _fixture.CreateMany<DistributionVM>();
            _bettypeServiceMock.Setup(_ => _.GetDistributionWithComparison(filter)).ReturnsAsync(distributionsData);

            var measurementDifferenceData = _fixture.CreateMany<MeasurementComparisonVM>();
            _bettypeServiceMock.Setup(_ => _.GetMeasurementDifferenceWithComparison(filter)).ReturnsAsync(measurementDifferenceData);

            var periodGrowthData = _fixture.CreateMany<MeasurementPeriodComparisonGrowthVM>();
            _bettypeServiceMock.Setup(_ => _.GetPeriodGrowthWithComparison(filter)).ReturnsAsync(periodGrowthData);

            var expected = new
            {
                data = new
                {
                    BaseCurrency = "USD",
                    MeasurementId = measurementId,
                    filter.CurrentPeriod,
                    filter.PreviousPeriod,
                    topDistributions,
                    DistributionData = measurementId != 3 ? distributionsData : null,
                    OverallMarginData = measurementId == 3 ? overallMarginData : null,
                    MeasurementDifferenceData = measurementDifferenceData,
                    GrowthData = new
                    {
                        PeriodGrowth = periodGrowthData,
                        MonthGrowth = new { }
                    }
                },
                isSuccess = true,
                msg = string.Empty
            };

            //Act
            var result = await _target.GetCombineData(filter);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                JsonConvert.SerializeObject(result.Value, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            ));
        }

        private BettypePerformanceFilterMetaModel FakeBettypePerformanceFilterMetaModel => new BettypePerformanceFilterMetaModel
        {
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel() { Id = 1 },
            BaseCurrencyName = "USD",
            CombineIds = Enumerable.Empty<string>()
        };
    }
}