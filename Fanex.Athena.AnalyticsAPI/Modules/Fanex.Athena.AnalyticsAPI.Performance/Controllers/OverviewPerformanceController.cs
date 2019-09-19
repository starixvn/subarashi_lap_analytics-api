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
        [Route("performance/overview/total")]
        public async Task<IActionResult> GetTotal([FromBody]PerformanceFilterMetalModel filter)
        {
            var result = await _overviewPerformanceService.GetTotalResultWithComparison(filter);
            return ToResult(result);
        }

        //[HttpPost]
        //[Route("performance/overview/top-change")]
        //public async Task<IActionResult> GetTopChange(GeneralFilterMetalModel filterParam)
        //{
        //    var result = await _overviewPerformanceRepo.GetTotal(filterParam);
        //    return ToResult(result);
        //}

        //[HttpPost]
        //[Route("performance/overview/growth")]
        //public async Task<IActionResult> GetGrowth(GeneralFilterMetalModel filterParam)
        //{
        //    var result = await _overviewPerformanceRepo.GetTotal(filterParam);
        //    return ToResult(result);
        //}
    }
}