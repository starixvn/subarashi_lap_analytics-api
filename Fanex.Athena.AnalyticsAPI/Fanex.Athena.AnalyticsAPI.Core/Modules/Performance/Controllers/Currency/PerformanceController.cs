using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    partial class PerformanceController
    {
        [HttpPost]
        [Route("performance/currency/total")]
        public async Task<IActionResult> GetCurrenyTotal(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetTotal(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/growth")]
        public async Task<IActionResult> GetCurrenyGrowth(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/distribution")]
        public async Task<IActionResult> GetCurrenyDistribution(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}