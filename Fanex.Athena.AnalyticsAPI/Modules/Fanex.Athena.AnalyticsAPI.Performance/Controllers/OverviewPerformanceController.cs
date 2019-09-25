using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewPerformanceService;

        public OverviewPerformanceController(IOverviewPerformanceService overviewPerformanceService) : base(null)
        {
            _overviewPerformanceService = overviewPerformanceService;
        }

        [HttpPost]
        [Route("performance/overview/summary")]
        public async Task<IActionResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewPerformanceService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/top-change")]
        public async Task<IActionResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var taskCurrency = _overviewPerformanceService.GetCurrencyTopChangeWithComparison(filter);
            var taskBettype = _overviewPerformanceService.GetBettypeTopChangeWithComparison(filter);
            var taskPlatform = _overviewPerformanceService.GetPlatformTopChangeWithComparison(filter);

            var results = await Task.WhenAll(taskCurrency, taskBettype, taskPlatform);

            return ToResult(results);
        }

        [HttpPost]
        [Route("performance/overview/growth")]
        public async Task<IActionResult> GetGrowth(PerformanceFilterMetaModel filterParam)
        {
            var result = await _overviewPerformanceService.GetGrowthWithComparison(filterParam);
            return ToResult(result);
        }
    }
}