using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewService;

        public OverviewPerformanceController(
            IContextService contextService,
            IOverviewPerformanceService overviewPerformanceService
            )
            : base(contextService)
        {
            _overviewService = overviewPerformanceService;
        }

        [HttpPost]
        [Route("api/performance/overview/summary")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/top-change")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _getTopChange(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/month-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetMonthGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData(PerformanceFilterMetaModel filter)
        {
            var topChangeTask = _getTopChange(filter);
            var summaryTask = _overviewService.GetSummaryWithComparison(filter);
            var periodGrowthTask = _overviewService.GetPeriodGrowthWithComparison(filter);

            var filterClone = MetaModelHelper.Clone(filter);
            filterClone.CurrentPeriod = filter.PreviousPeriod;
            var prevDetailMonthGrowthTask = _overviewService.GetDetailMonthGrowthWithComparison(filterClone);

            var currentDetailMonthGrowthTask = _overviewService.GetDetailMonthGrowthWithComparison(filter);

            var topChangeData = await topChangeTask;
            var summaryData = await summaryTask;
            var periodGrowth = await periodGrowthTask;

            var prevDetailMonthGrowth = await prevDetailMonthGrowthTask;
            var currentDetailMonthGrowth = await currentDetailMonthGrowthTask;

            return ToResult(new
            {
                baseCurrency = filter.BaseCurrencyName,
                measurementId = filter.Measurement.Id,
                summaryData,
                growthData = new
                {
                    monthGrowth = new [] 
                    {
                        prevDetailMonthGrowth,
                        currentDetailMonthGrowth
                    },
                    periodGrowth
                },
                topChangeData
            });
        }

        private async Task<object> _getTopChange(PerformanceFilterMetaModel filter)
        {
            var taskCurrency = _overviewService.GetCurrencyTopChangeWithComparison(filter);
            var taskBettype = _overviewService.GetBettypeTopChangeWithComparison(filter);
            var taskPlatform = _overviewService.GetPlatformTopChangeWithComparison(filter);
            var taskProduct = _overviewService.GetProductTopChangeWithComparison(filter);

            var currencyTopChange = await taskCurrency;
            var bettypeTopChange = await taskBettype;
            var platformTopChange = await taskPlatform;
            var productTopChange = await taskProduct;

            return new
            {
                currencyTopChange,
                bettypeTopChange,
                platformTopChange,
                productTopChange
            };
        }
    }
}