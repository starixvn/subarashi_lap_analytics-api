using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Constants;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Attributes;
using Fanex.Athena.AnalyticsAPI.Core.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Extensions;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PerformanceTab)]
    public class ProductPerformanceController : ControllerBaseCustom
    {
        private readonly IProductPerformanceService _productService;
        private readonly ITranslationService _translationService;

        public ProductPerformanceController(
            IContextService contextService,
            IProductPerformanceService productService,
            ITranslationService translationService
            )
            : base(contextService)
        {
            _productService = productService;
            _translationService = translationService;
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

            _translationService.Translate(filter.TopDistributions, TranslationConst.ProductType);

            var isMargin = filter.Measurement.Id == MeasurementConst.MARGIN;

            var (measurementDifferenceData, periodGrowthData, distributionData, overallMarginData) = await TaskExtension.WhenAllAsync(
                              _productService.GetMeasurementDifferenceWithComparison(filter),
                              _productService.GetPeriodGrowthWithComparison(filter),
                              !isMargin ? _productService.GetDistributionWithComparison(filter) : Task.FromResult<IEnumerable<DistributionVM>>(null),
                              isMargin ? _productService.GetOverallMeasurement(filter) : Task.FromResult<IEnumerable<OverallMeasurementVM>>(null));

            _translationService.Translate(distributionData, TranslationConst.NormalType);
            _translationService.Translate(overallMarginData, TranslationConst.NormalType);

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