using Fanex.Athena.AnalyticsAPI.Core.Controllers;
using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
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
        [Route("performance/currency/total")]
        public async Task<IActionResult> GetTotal(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetTotal(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/growth")]
        public async Task<IActionResult> GetGrowth(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/currency/distribution")]
        public async Task<IActionResult> GetDistribution(GeneralFilterParam filterParam)
        {
            var result = await this._currencyPerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}