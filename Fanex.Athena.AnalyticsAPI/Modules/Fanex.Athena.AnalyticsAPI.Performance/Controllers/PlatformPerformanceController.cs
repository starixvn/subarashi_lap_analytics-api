using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels.Constant;
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

        public PlatformPerformanceController(
            IContextService contextService,
            IPlatformPerformanceService platformService
            )
            : base(contextService)
        {
            _platformService = platformService;
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

            var distributions = null as Task<IEnumerable<DistributionVM>>;
            var overallMargin = null as Task<IEnumerable<OverallMeasurementVM>>;

            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                overallMargin = _platformService.GetOverallMeasurement(filter);
            }
            else
            {
                distributions = _platformService.GetDistributionWithComparison(filter);
            }

            var taskMeasurementDifference = _platformService.GetMeasurementDifferenceWithComparison(filter);
            var taskPeriodGrowth = _platformService.GetPeriodGrowthWithComparison(filter);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                filter.TopDistributions,
                DistributionData = distributions != null ? await distributions : null,
                OverallMarginData = overallMargin != null ? await overallMargin : null,
                MeasurementDifferenceData = await taskMeasurementDifference,
                GrowthData = new
                {
                    PeriodGrowth = await taskPeriodGrowth,
                    MonthGrowth = new {}
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