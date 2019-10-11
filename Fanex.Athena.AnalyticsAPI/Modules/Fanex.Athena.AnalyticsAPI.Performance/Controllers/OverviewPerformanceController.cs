using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewService;

        public OverviewPerformanceController(
            IContextService contextService,
            IOverviewPerformanceService overviewPerformanceService
            )
            : base(contextService)
        {
            _overviewService = overviewPerformanceService;
        }

        [HttpPost]
        [Route("api/performance/overview/summary")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/top-change")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _getTopChange(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/period-growth")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetPeriodGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/month-growth")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetMonthGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetCombineData(PerformanceFilterMetaModel filter)
        {
            var summaryTask = _overviewService.GetSummaryWithComparison(filter);
            var periodgrowthTask = _overviewService.GetPeriodGrowthWithComparison(filter);
            var monthGrowthTask = _overviewService.GetMonthGrowthWithComparison(filter);
            var topChangeTask = _getTopChange(filter);

            var summaryData = await summaryTask;
            var periodGrowth = await periodgrowthTask;
            var monthGrowth = await monthGrowthTask;
            var topChangeData = await topChangeTask;

            return ToResult(new
            {
                baseCurrency = filter.BaseCurrencyName,
                measurementId = filter.Measurement.Id,
                summaryData,
                growthData = new
                {
                    monthGrowth,
                    periodGrowth
                },
                topChangeData
            });
        }

        private async Task<object> _getTopChange(PerformanceFilterMetaModel filter)
        {
            var taskCurrency = _overviewService.GetCurrencyTopChangeWithComparison(filter);
            var taskBettype = _overviewService.GetBettypeTopChangeWithComparison(filter);
            var taskPlatform = _overviewService.GetPlatformTopChangeWithComparison(filter);
            var taskProduct = _overviewService.GetProductTopChangeWithComparison(filter);

            var currencyTopChange = await taskCurrency;
            var bettypeTopChange = await taskBettype;
            var platformTopChange = await taskPlatform;
            var productTopChange = await taskProduct;

            return new
            {
                currencyTopChange,
                bettypeTopChange,
                platformTopChange,
                productTopChange
            };
        }
    }
}