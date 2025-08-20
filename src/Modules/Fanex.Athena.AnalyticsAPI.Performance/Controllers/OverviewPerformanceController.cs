namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Controllers;
    using Common.Extensions;
    using Common.Helpers;
    using Common.Services;
    using Core.Attributes;
    using Core.Constants;
    using Core.Helpers;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModels;
    using Services;

    [ApiController]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PerformanceTab)]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewService;
        private readonly ITranslationService _translationService;
        private readonly IAuthInfoService _authInfoService;

        public OverviewPerformanceController(
            IContextService contextService,
            IOverviewPerformanceService overviewPerformanceService,
            ITranslationService translationService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _overviewService = overviewPerformanceService;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [Route("api/performance/overview/summary")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetSummary([FromBody] PerformanceFilterMetaModel filter)
        {
            var result = await _overviewService.GetSummaryWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/overview/top-change")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetTopChange([FromBody] PerformanceFilterMetaModel filter)
        {
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);

            var result = await _getTopChange(filter, isHideSaba);
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
        public async Task<JsonResult> GetCombineData([FromBody] PerformanceFilterMetaModel filter)
        {
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);
            var topChangeTask = _getTopChange(filter, isHideSaba);
            var summaryTask = _overviewService.GetSummaryWithComparison(filter);
            var periodGrowthTask = _overviewService.GetPeriodGrowthWithComparison(filter);

            var filterClone = filter.Clone();
            filterClone.CurrentPeriod = filter.PreviousPeriod;
            var prevDetailMonthGrowthTask = _overviewService.GetDetailMonthGrowthWithComparison(filterClone);
            var currentDetailMonthGrowthTask = _overviewService.GetDetailMonthGrowthWithComparison(filter);

            var topChangeData = await topChangeTask;
            var summaryData = await summaryTask;
            var periodGrowthData = await periodGrowthTask;

            var prevPeriodMonthGrowthData = await prevDetailMonthGrowthTask;
            var currentPeriodMonthGrowthData = await currentDetailMonthGrowthTask;

            _translationService.Translate(periodGrowthData, isHideSaba, TranslationConst.NormalType);

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

        private async Task<object> _getTopChange(PerformanceFilterMetaModel filter, bool isHideSaba)
        {
            var (currencyTopChange, bettypeTopChange, platformTopChange, productTopChange) = await TaskExtension.WhenAllAsync(
                     _overviewService.GetCurrencyTopChangeWithComparison(filter),
                     _overviewService.GetBettypeTopChangeWithComparison(filter),
                     _overviewService.GetPlatformTopChangeWithComparison(filter),
                     _overviewService.GetProductTopChangeWithComparison(filter)
                );

            _translationService.Translate(bettypeTopChange, isHideSaba, TranslationConst.BettypeType);
            _translationService.Translate(platformTopChange, isHideSaba, TranslationConst.PlatformType);
            _translationService.Translate(productTopChange, isHideSaba, TranslationConst.ProductType);

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