using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        //private readonly IOverviewPerformanceService _overviewPerformanceService;
        private readonly IOverviewPerformanceRepo _overviewPerformanceRepo;

        public OverviewPerformanceController(IOverviewPerformanceRepo overviewPerformanceRepo) : base(null)
        {
            //_overviewPerformanceService = overviewPerformanceService;
            _overviewPerformanceRepo = overviewPerformanceRepo;
        }

        [HttpPost]
        [Route("performance/overview/total")]
        public async Task<IActionResult> GetTotal(GeneralFilterParam filterParam)
        {
            var result = await _overviewPerformanceRepo.GetTurnover(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/top-change")]
        public async Task<IActionResult> GetTopChange(GeneralFilterParam filterParam)
        {
            var result = await _overviewPerformanceRepo.GetTurnover(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/overview/growth")]
        public async Task<IActionResult> GetGrowth(GeneralFilterParam filterParam)
        {
            var result = await _overviewPerformanceRepo.GetTurnover(filterParam);
            return ToResult(result);
        }
    }
}