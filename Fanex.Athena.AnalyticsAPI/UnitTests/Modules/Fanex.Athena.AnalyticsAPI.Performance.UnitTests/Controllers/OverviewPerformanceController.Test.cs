using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Services;
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
    public class OverviewPerformanceController_Test
    {
        private readonly OverviewPerformanceController _target;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<IOverviewPerformanceService> _overviewServiceMock;
        private readonly Mock<ITranslationService> _translationServiceMock;
        private readonly Mock<IAuthInfoService> _authInfoServiceMock;
        private readonly Fixture _fixture;

        public OverviewPerformanceController_Test()
        {
            _contextServiceMock = new Mock<IContextService>();
            _overviewServiceMock = new Mock<IOverviewPerformanceService>();
            _translationServiceMock = new Mock<ITranslationService>();
            _authInfoServiceMock = new Mock<IAuthInfoService>();
            _fixture = new Fixture();
            _target = new OverviewPerformanceController(_contextServiceMock.Object, _overviewServiceMock.Object, _translationServiceMock.Object, _authInfoServiceMock.Object);
        }

        [Fact]
        public async Task GetCombineData_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var filter = FakePerformanceFilterMetaModel;
            var topChangeCurrencyData = _fixture.CreateMany<TopChangeVM>();
            var topChangeBettypeData = _fixture.CreateMany<TopChangeVM>();
            var topChangePlatformData = _fixture.CreateMany<TopChangeVM>();
            var topChangeProductData = _fixture.CreateMany<TopChangeVM>();
            _overviewServiceMock.Setup(_ => _.GetCurrencyTopChangeWithComparison(filter)).ReturnsAsync(topChangeCurrencyData);
            _overviewServiceMock.Setup(_ => _.GetBettypeTopChangeWithComparison(filter)).ReturnsAsync(topChangeBettypeData);
            _overviewServiceMock.Setup(_ => _.GetPlatformTopChangeWithComparison(filter)).ReturnsAsync(topChangePlatformData);
            _overviewServiceMock.Setup(_ => _.GetProductTopChangeWithComparison(filter)).ReturnsAsync(topChangeProductData);

            var topChangeData = new
            {
                currencyTopChange = topChangeCurrencyData,
                bettypeTopChange = topChangeBettypeData,
                platformTopChange = topChangePlatformData,
                productTopChange = topChangeProductData
            };

            var summaryData = _fixture.CreateMany<SummaryVM>();
            _overviewServiceMock.Setup(_ => _.GetSummaryWithComparison(filter)).ReturnsAsync(summaryData);

            var periodGrowthData = _fixture.CreateMany<MeasurementGrowthVM>();
            _overviewServiceMock.Setup(_ => _.GetPeriodGrowthWithComparison(filter)).ReturnsAsync(periodGrowthData);

            var prePeriodMonthGrowthData = _fixture.CreateMany<MeasurementGrowthVM>();
            var tempFilter = null as PerformanceFilterMetaModel;
            _overviewServiceMock.Setup(_ => _.GetDetailMonthGrowthWithComparison(It.IsAny<PerformanceFilterMetaModel>()))
                .Callback<PerformanceFilterMetaModel>(_ => tempFilter = _)
                .ReturnsAsync(() => tempFilter.CurrentPeriod.Equals(filter.PreviousPeriod) ? prePeriodMonthGrowthData : null);

            var currentPeriodMonthGrowthData = _fixture.CreateMany<MeasurementGrowthVM>();
            _overviewServiceMock.Setup(_ => _.GetDetailMonthGrowthWithComparison(filter)).ReturnsAsync(currentPeriodMonthGrowthData);

            var expected = new
            {
                data = new
                {
                    baseCurrency = "USD",
                    measurementId = 1,
                    summaryData,
                    growthData = new
                    {
                        monthGrowth = new[]
                        {
                            prePeriodMonthGrowthData,
                            currentPeriodMonthGrowthData
                        },
                        periodGrowth = periodGrowthData
                    },
                    topChangeData
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

        private PerformanceFilterMetaModel FakePerformanceFilterMetaModel => new PerformanceFilterMetaModel
        {
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel() { Id = 1 },
            BaseCurrencyName = "USD"
        };
    }
}