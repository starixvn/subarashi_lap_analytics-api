using Fanex.Athena.AnalyticsAPI.Core.Controllers;
using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class BetTypePerformanceController : ControllerBaseCustom
    {
        private readonly IBetTypePerformanceService _betTypePerformanceService;

        public BetTypePerformanceController(IBetTypePerformanceService betTypePerformanceService)
        {
            _betTypePerformanceService = betTypePerformanceService;
        }

        [HttpPost]
        [Route("performance/bettype/breakdown")]
        public async Task<IActionResult> GetBetTypeTotal(GeneralFilterParam filterParam)
        {
            var result = await this._betTypePerformanceService.GetBreakdown(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/bettype/growth")]
        public async Task<IActionResult> GetBetTypeGrowth(GeneralFilterParam filterParam)
        {
            var result = await this._betTypePerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/bettype/distribution")]
        public async Task<IActionResult> GetBetTypeDistribution(GeneralFilterParam filterParam)
        {
            var result = await this._betTypePerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}