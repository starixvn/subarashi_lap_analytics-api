namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Controllers;
    using Common.Extensions;
    using Common.Services;
    using Constants;
    using Core.Attributes;
    using Core.Constants;
    using Core.Helpers;
    using Core.Services;
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModels;
    using Models.ViewModels;
    using Services;

    [ApiController]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PerformanceTab)]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyService;
        private readonly ITranslationService _translationService;
        private readonly IAuthInfoService _authInfoService;

        public CurrencyPerformanceController(
            IContextService contextService,
            ICurrencyPerformanceService currencyService,
            ITranslationService translationService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _currencyService = currencyService;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [Route("api/performance/currency/distribution")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDistribution([FromBody] CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/diffences")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDiffences([FromBody] CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth([FromBody] CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency/month-growth-all")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody] CurrenyPerformanceFilterMetaModel filter)
        {
            var result = await _currencyService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/currency")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody] CurrenyPerformanceFilterMetaModel filter)
        {
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);
            await filter.BindTopDistributionsToFilterMetaModel(_currencyService);

            _translationService.Translate(filter.TopDistributions, isHideSaba, TranslationConst.CurrencyType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _currencyService.GetMeasurementDifferenceWithComparison(filter),
                              _currencyService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _currencyService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _currencyService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

            _translationService.Translate(distributionData, isHideSaba, TranslationConst.NormalType);
            _translationService.Translate(overallMarginData, isHideSaba, TranslationConst.NormalType);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                filter.TopDistributions,
                distributionData,
                overallMarginData,
                measurementDifferenceData,
                GrowthData = new
                {
                    PeriodGrowth = periodGrowthData,
                    MonthGrowth = new { }
                }
            });
        }
    }
}