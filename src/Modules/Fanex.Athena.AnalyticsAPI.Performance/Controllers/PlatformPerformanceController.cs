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
    public class PlatformPerformanceController : ControllerBaseCustom
    {
        private readonly IPlatformPerformanceService _platformService;
        private readonly ITranslationService _translationService;
        private readonly IAuthInfoService _authInfoService;

        public PlatformPerformanceController(
            IContextService contextService,
            IPlatformPerformanceService platformService,
            ITranslationService translationService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _platformService = platformService;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [Route("api/performance/platform/distribution")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDistribution([FromBody] PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/diffences")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDiffences([FromBody] PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth([FromBody] PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform/month-growth-all")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody] PlatformPerformanceFilterMetaModel filter)
        {
            var result = await _platformService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/platform")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody] PlatformPerformanceFilterMetaModel filter)
        {
            await filter.BindTopDistributionsToFilterMetaModel(_platformService);
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);
            _translationService.Translate(filter.TopDistributions, isHideSaba, TranslationConst.PlatformType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _platformService.GetMeasurementDifferenceWithComparison(filter),
                              _platformService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _platformService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _platformService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

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