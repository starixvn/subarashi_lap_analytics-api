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
        public async Task<JsonResult> GetDistribution([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/month-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetMonthGrowth([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            var result = await _currencyService.GetDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]CurrenyPerformanceFilterMetalModel filter)
        {
            return ToResult(new { });
        }
    }
}