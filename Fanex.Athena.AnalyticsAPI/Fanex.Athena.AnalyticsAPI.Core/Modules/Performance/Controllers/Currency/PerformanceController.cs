using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    partial class PerformanceController
    {
        [HttpGet]
        [Route("performance/currency/total")]
        public IActionResult GetCurrenyTotal()
        {
            return ToResult(new { Total = 10000 });
        }

        [HttpGet]
        [Route("performance/currency/growth")]
        public IActionResult GetCurrenyGrowth()
        {
            return ToResult(new { Total = 10000 });
        }

        [HttpGet]
        [Route("performance/currency/distribution")]
        public IActionResult GetCurrenyDistribution()
        {
            return ToResult(new { Total = 10000 });
        }
    }
}