using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    [ONEBookAuthorization(Permissions = PermissionConst.PERFORMANCE_TAB)]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewService;
        private readonly ITranslationService _translationService;

        public OverviewPerformanceController(
            IContextService contextService,
            IOverviewPerformanceService overviewPerformanceService,
            ITranslationService translationService
            )
            : base(contextService)
        {
            _overviewService = overviewPerformanceService;
            _translationService = translationService;
        }

        [HttpPost]
        [Route("api/performance/overview/summary")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetSummary([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/top-change")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetTopChange([FromBody]PerformanceFilterMetaModel filter)
        {
            var result = await _getTopChange(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/month-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetMonthGrowth(PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody]PerformanceFilterMetaModel filter)
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
            var periodGrowthData = await periodGrowthTask;

            var prevPeriodMonthGrowthData = await prevDetailMonthGrowthTask;
            var currentPeriodMonthGrowthData = await currentDetailMonthGrowthTask;

            _translationService.Translate(periodGrowthData, TranslationConst.NormalType);

            return ToResult(new
            {
                baseCurrency = filter.BaseCurrencyName,
                measurementId = filter.Measurement.Id,
                summaryData,
                growthData = new
                {
                    monthGrowth = new[]
                    {
                        prevPeriodMonthGrowthData,
                        currentPeriodMonthGrowthData
                    },
                    periodGrowth = periodGrowthData
                },
                topChangeData
            });
        }

        private async Task<object> _getTopChange(PerformanceFilterMetaModel filter)
        {
            var (currencyTopChange, bettypeTopChange, platformTopChange, productTopChange) = await TaskExtension.WhenAllAsync(
                     _overviewService.GetCurrencyTopChangeWithComparison(filter),
                     _overviewService.GetBettypeTopChangeWithComparison(filter),
                     _overviewService.GetPlatformTopChangeWithComparison(filter),
                     _overviewService.GetProductTopChangeWithComparison(filter)
                );

            _translationService.Translate(bettypeTopChange, TranslationConst.BettypeType);
            _translationService.Translate(platformTopChange, TranslationConst.PlatformType);
            _translationService.Translate(productTopChange, TranslationConst.ProductType);

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