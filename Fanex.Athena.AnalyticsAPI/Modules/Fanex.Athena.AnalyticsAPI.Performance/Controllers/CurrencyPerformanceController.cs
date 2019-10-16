using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyService;

        public CurrencyPerformanceController(
            IContextService contextService,
            ICurrencyPerformanceService currencyService
            )
            : base(contextService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        [Route("api/performance/currency/distribution")]
        [AutoBindMetaModel]
        public async Task<IActionResult> GetDistribution([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }
    }
}