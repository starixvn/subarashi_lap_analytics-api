using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class PlatformPerformanceController : ControllerBaseCustom
    {
        private readonly IPlatformPerformanceService _platformService;
        private readonly ITranslationService _translationService;

        public PlatformPerformanceController(
            IContextService contextService,
            IPlatformPerformanceService platformService,
            ITranslationService translationService
            )
            : base(contextService)
        {
            _platformService = platformService;
            _translationService = translationService;
        }

        [HttpPost]
        [Route("api/performance/platform/distribution")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDistribution([FromBody]PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/month-growth-all")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]PlatformPerformanceFilterMetaModel filter)
        {
            await BindTopDistributionsToFilterMetaModel(filter);
            _translationService.Translate(filter.TopDistributions, filter.LangCode, TranslationConst.PlatformType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _platformService.GetMeasurementDifferenceWithComparison(filter),
                              _platformService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _platformService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _platformService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

            _translationService.Translate(distributionData, filter.LangCode, TranslationConst.NormalType);
            _translationService.Translate(overallMarginData, filter.LangCode, TranslationConst.NormalType);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                filter.TopDistributions,
                distributionData,
                overallMarginData,
                measurementDifferenceData,
                GrowthData = new
                {
                    PeriodGrowth = periodGrowthData,
                    MonthGrowth = new { }
                }
            });
        }

        private async Task BindTopDistributionsToFilterMetaModel(PerformanceFilterMetaModel filter)
        {
            var topDistributions = await _platformService.GetTopDistributions(filter);
            filter.TopDistributions = topDistributions.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName
            });
        }
    }
}