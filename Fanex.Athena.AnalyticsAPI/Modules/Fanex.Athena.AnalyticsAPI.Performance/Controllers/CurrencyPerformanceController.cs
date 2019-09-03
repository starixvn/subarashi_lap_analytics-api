using Fanex.Athena.AnalyticsAPI.Core.Controllers;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ClientParams;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyPerformanceService;

        public CurrencyPerformanceController(ICurrencyPerformanceService currencyPerformanceService)
        {
            _currencyPerformanceService = currencyPerformanceService;
        }

        [HttpPost]
        [Route("performance/currency/breakdown")]
        public async Task<IActionResult> GetBreakdown(PerformanceFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetBreakdown(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/growth")]
        public async Task<IActionResult> GetGrowth(PerformanceFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/distribution")]
        public async Task<IActionResult> GetDistribution(PerformanceFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}