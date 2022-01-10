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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Controllers
{
    public class PlatformPerformanceController_Test
    {
        private readonly PlatformPerformanceController _target;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<IPlatformPerformanceService> _platformServiceMock;
        private readonly Mock<ITranslationService> _translationServiceMock;

        private readonly Fixture _fixture;

        public PlatformPerformanceController_Test()
        {
            _contextServiceMock = new Mock<IContextService>();
            _platformServiceMock = new Mock<IPlatformPerformanceService>();
            _translationServiceMock = new Mock<ITranslationService>();
            _fixture = new Fixture();
            _target = new PlatformPerformanceController(_contextServiceMock.Object, _platformServiceMock.Object, _translationServiceMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetCombineData_WhenCall_ReturnCorrectResult(int measurementId)
        {
            //Arrange
            var filter = FakePlatformPerformanceFilterMetaModel;
            filter.Measurement.Id = measurementId;

            var topDistributionsData= _fixture.CreateMany<(string topDistributionId, string topDistributionName)>();
            var topDistributions = topDistributionsData.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName,
                CurrentPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } },
                PreviousPeriodDistributions = new List<IdentityMetaModel> { new IdentityMetaModel { Id = _.topDistributionId, Name = _.topDistributionName } }
            }).ToList();

            _platformServiceMock.Setup(_ => _.GetTopDistributions(filter, 5)).ReturnsAsync(topDistributionsData);

            var overallMarginData = _fixture.CreateMany<OverallMeasurementVM>();
            _platformServiceMock.Setup(_ => _.GetOverallMeasurement(filter)).ReturnsAsync(overallMarginData);

            var distributionsData = _fixture.CreateMany<DistributionVM>();
            _platformServiceMock.Setup(_ => _.GetDistributionWithComparison(filter)).ReturnsAsync(distributionsData);

            var measurementDifferenceData = _fixture.CreateMany<MeasurementComparisonVM>();
            _platformServiceMock.Setup(_ => _.GetMeasurementDifferenceWithComparison(filter)).ReturnsAsync(measurementDifferenceData);

            var periodGrowthData = _fixture.CreateMany<MeasurementPeriodComparisonGrowthVM>();
            _platformServiceMock.Setup(_ => _.GetPeriodGrowthWithComparison(filter)).ReturnsAsync(periodGrowthData);

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
                        MonthGrowth = new {}
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

        private PlatformPerformanceFilterMetaModel FakePlatformPerformanceFilterMetaModel => new PlatformPerformanceFilterMetaModel
        {
            CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
            PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
            Measurement = new Common.Models.MetaModels.SelectMetaModel() { Id = 1 },
            BaseCurrencyName = "USD",
            Platforms = Enumerable.Empty<string>()
        };
    }
}