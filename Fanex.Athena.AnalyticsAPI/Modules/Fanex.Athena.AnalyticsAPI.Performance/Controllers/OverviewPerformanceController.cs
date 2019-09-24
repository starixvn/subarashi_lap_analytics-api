using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewPerformanceService;
        private readonly IDaxQueryContentService _service;


        public OverviewPerformanceController(IOverviewPerformanceService overviewPerformanceService, IDaxQueryContentService service) : base(null)
        {
            _overviewPerformanceService = overviewPerformanceService;
            _service = service;
        }

        [HttpPost]
        [Route("performance/overview/summary")]
        public async Task<IActionResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewPerformanceService.GetSummaryResultWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/top-change")]
        public async Task<IActionResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewPerformanceService.GetTopChangeResultWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/growth")]
        public async Task<IActionResult> GetGrowth(PerformanceFilterMetaModel filterParam)
        {
            var result = await _overviewPerformanceService.GetGrowthResultWithComparison(filterParam);
            return ToResult(result);
        }
    }

    public class TestCriteria : ICriteria
    {
        public string SiteName { get; set; } = "12BET";
        public int BaseCurrencyId { get; set; } = 2;
        public string FromDate { get; set; } = "01/01/2019";
        public string ToDate { get; set; } = "08/31/2019";

        [SpParam(Ignored = true)]
        public int MyProperty { get; set; } = 0;


        public string GetSettingKey()
        {
            return "LAP_Performance_OverviewSummaryBySite";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}