using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    partial class PerformanceController
    {
        [HttpGet]
        [Route("performance/bettype/total")]
        public async Task<IActionResult> GetBetTypeTotal()
        {
            var result = await this._betTypePerformanceService.GetTotal();
            return ToResult(result);
        }

        [HttpGet]
        [Route("performance/bettype/growth")]
        public async Task<IActionResult> GetBetTypeGrowth()
        {
            var result = await this._betTypePerformanceService.GetGrowth();
            return ToResult(result);
        }

        [HttpGet]
        [Route("performance/bettype/distribution")]
        public async Task<IActionResult> GetBetTypeDistribution()
        {
            var result = await this._betTypePerformanceService.GetDistribution();
            return ToResult(result);
        }
    }
}