using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
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
    public class BettypePerformanceController : ControllerBaseCustom
    {
        private readonly IBettypePerformanceService _bettypeService;

        public BettypePerformanceController(
            IContextService contextService,
            IBettypePerformanceService bettypeService
            )
            : base(contextService)
        {
            _bettypeService = bettypeService;
        }

        [HttpPost]
        [Route("api/performance/bettype/distribution")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDistribution([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/month-growth-all")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]BettypePerformanceFilterMetaModel filter)
        {
            await BindTopDistributionsToFilterMetaModel(filter);

            var distributions = null as Task<IEnumerable<DistributionVM>>;
            var overallMargin = null as Task<IEnumerable<OverallMeasurementVM>>;

            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                overallMargin = _bettypeService.GetOverallMeasurement(filter);
            }
            else
            {
                distributions = _bettypeService.GetDistributionWithComparison(filter);
            }

            var taskMeasurementDifference = _bettypeService.GetMeasurementDifferenceWithComparison(filter);
            var taskPeriodGrowth = _bettypeService.GetPeriodGrowthWithComparison(filter);

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
            const int numberOfTop = 10;
            var topDistributions = await _bettypeService.GetTopDistributions(filter, numberOfTop);
            filter.TopDistributions = topDistributions.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName
            });
        }
    }
}