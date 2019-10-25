using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
            var distributions = await _currencyService.GetDistributionWithComparison(filter);
            filter.TopDistributions = distributions.ElementAt(1).Distributions.Select(_ => Tuple.Create(_.Id, _.Name));

            var taskMeasurementDifference = _currencyService.GetMeasurementDifferenceWithComparison(filter);
            var taskPeriodGrowth = _currencyService.GetPeriodGrowthWithComparison(filter);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                Distributions = distributions,
                MeasurementDifference = await taskMeasurementDifference,
                GrowthData = new
                {
                    PeriodGrowth = await taskPeriodGrowth, 
                    MonthGrowth = new {}
                }
            });
        }

    }
}