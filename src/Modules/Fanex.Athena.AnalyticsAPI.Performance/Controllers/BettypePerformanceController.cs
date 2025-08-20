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
    public class BettypePerformanceController : ControllerBaseCustom
    {
        private readonly IBettypePerformanceService _bettypeService;
        private readonly ITranslationService _translationService;
        private readonly IAuthInfoService _authInfoService;

        public BettypePerformanceController(
            IContextService contextService,
            IBettypePerformanceService bettypeService,
            ITranslationService translationService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _bettypeService = bettypeService;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [Route("api/performance/bettype/distribution")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDistribution([FromBody] BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/diffences")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDiffences([FromBody] BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth([FromBody] BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype/month-growth-all")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody] BettypePerformanceFilterMetaModel filter)
        {
            var result = await _bettypeService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/bettype")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody] BettypePerformanceFilterMetaModel filter)
        {
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);
            const int numberOfTopBettype = 10;
            await filter.BindTopDistributionsToFilterMetaModel(_bettypeService, numberOfTopBettype);

            _translationService.Translate(filter.TopDistributions, isHideSaba, TranslationConst.BettypeType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _bettypeService.GetMeasurementDifferenceWithComparison(filter),
                              _bettypeService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _bettypeService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _bettypeService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

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