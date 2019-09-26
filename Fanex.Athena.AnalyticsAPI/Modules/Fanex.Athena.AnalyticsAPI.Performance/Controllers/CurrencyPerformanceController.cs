using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyPerformanceService;

        public CurrencyPerformanceController(ICurrencyPerformanceService currencyPerformanceService) : base(null)
        {
            _currencyPerformanceService = currencyPerformanceService;
        }

        //[HttpPost]
        //[Route("performance/currency/breakdown")]
        //public async Task<IActionResult> GetBreakdown(PerformanceFilterMetalModel filterParam)
        //{
        //    var result = await this._currencyPerformanceService.GetBreakdown(filterParam);
        //    return ToResult(result);
        //}

        //[HttpPost]
        //[Route("performance/currency/growth")]
        //public async Task<IActionResult> GetGrowth(PerformanceFilterMetalModel filterParam)
        //{
        //    var result = await this._currencyPerformanceService.GetGrowth(filterParam);
        //    return ToResult(result);
        //}

        //[HttpPost]
        //[Route("performance/currency/distribution")]
        //public async Task<IActionResult> GetDistribution(PerformanceFilterMetalModel filterParam)
        //{
        //    var result = await this._currencyPerformanceService.GetDistribution(filterParam);
        //    return ToResult(result);
        //}
    }
}