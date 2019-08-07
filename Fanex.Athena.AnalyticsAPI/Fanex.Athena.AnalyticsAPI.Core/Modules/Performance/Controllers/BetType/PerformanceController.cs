using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    partial class PerformanceController
    {
        [HttpPost]
        [Route("performance/bettype/total")]
        public async Task<IActionResult> GetBetTypeTotal(GeneralFilterParam filterParam)
        {
            var result = await this._betTypePerformanceService.GetTotal(filterParam);
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