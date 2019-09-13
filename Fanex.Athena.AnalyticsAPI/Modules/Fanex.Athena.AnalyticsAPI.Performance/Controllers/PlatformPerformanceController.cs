using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class PlatformPerformanceController : ControllerBaseCustom
    {
        private readonly IPlatformPerformanceService _platformPerformanceService;

        public PlatformPerformanceController(IPlatformPerformanceService platformPerformanceService) : base(null)
        {
            _platformPerformanceService = platformPerformanceService;
        }

        [HttpPost]
        [Route("performance/platform/breakdown")]
        public async Task<IActionResult> GetTotal(GeneralFilterParam filterParam)
        {
            var result = await this._platformPerformanceService.GetBreakdown(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/platform/growth")]
        public async Task<IActionResult> GetGrowth(GeneralFilterParam filterParam)
        {
            var result = await this._platformPerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/platform/distribution")]
        public async Task<IActionResult> GetDistribution(GeneralFilterParam filterParam)
        {
            var result = await this._platformPerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}