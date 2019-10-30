using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels.Constant;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyService;

        public CurrencyPerformanceController(
            IContextService contextService,
            ICurrencyPerformanceService currencyService
            )
            : base(contextService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        [Route("api/performance/currency/distribution")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDistribution([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/month-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetMonthGrowth([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/month-growth-all")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            filter.TopDistributions = await _currencyService.GetTopDistributions(filter);

            var distributions = null as Task<IEnumerable<DistributionVM>>;
            var overallMargin = null as Task<IEnumerable<OverallMeasurementVM>>;

            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                overallMargin = _currencyService.GetOverallMeasurement(filter);
            }
            else
            {
                distributions = _currencyService.GetDistributionWithComparison(filter);
            }

            var taskMeasurementDifference = _currencyService.GetMeasurementDifferenceWithComparison(filter);
            var taskPeriodGrowth = _currencyService.GetPeriodGrowthWithComparison(filter);

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
    }
}