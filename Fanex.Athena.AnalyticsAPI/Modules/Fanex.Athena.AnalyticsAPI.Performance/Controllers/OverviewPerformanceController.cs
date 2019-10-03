using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [Route("performance/overview/summary")]
        public async Task<IActionResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/top-change")]
        public async Task<IActionResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _getTopChange(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/period-growth")]
        public async Task<IActionResult> GetPeriodGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/month-growth")]
        public async Task<IActionResult> GetMonthGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview")]
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
                baseCurrency = filter.BaseCurrency.Name,
                measurementId = filter.Measurement.Id,
                summaryData,
                growthData = new {
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
            //var taskProduct = _overviewService.GetProductTopChangeWithComparison(filter);

            var currencyTopChange = await taskCurrency;
            var bettypeTopChange = await taskBettype;
            var platformTopChange = await taskPlatform;
            //var productTopeChange = await taskProduct;

            return new
            {
                currencyTopChange,
                bettypeTopChange,
                platformTopChange,
                //productTopeChange
            };
        }
    }
}