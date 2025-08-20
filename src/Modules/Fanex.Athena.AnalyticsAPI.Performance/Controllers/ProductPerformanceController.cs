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
    public class ProductPerformanceController : ControllerBaseCustom
    {
        private readonly IProductPerformanceService _productService;
        private readonly ITranslationService _translationService;
        private readonly IAuthInfoService _authInfoService;

        public ProductPerformanceController(
            IContextService contextService,
            IProductPerformanceService productService,
            ITranslationService translationService,
            IAuthInfoService authInfoService
            )
            : base(contextService)
        {
            _productService = productService;
            _translationService = translationService;
            _authInfoService = authInfoService;
        }

        [HttpPost]
        [Route("api/performance/product/distribution")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDistribution([FromBody] ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/diffences")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetDiffences([FromBody] ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/period-growth")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetPeriodGrowth([FromBody] ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/month-growth-all")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody] ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product")]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<JsonResult> GetCombineData([FromBody] ProductPerformanceFilterMetaModel filter)
        {
            await filter.BindTopDistributionsToFilterMetaModel(_productService);
            var isHideSaba = SabaHelper.IsHideSaba(_authInfoService.SpecialPlatform);

            _translationService.Translate(filter.TopDistributions, isHideSaba, TranslationConst.ProductType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _productService.GetMeasurementDifferenceWithComparison(filter),
                              _productService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _productService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _productService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

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