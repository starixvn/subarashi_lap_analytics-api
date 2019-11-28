using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class ProductPerformanceController : ControllerBaseCustom
    {
        private readonly IProductPerformanceService _productService;

        public ProductPerformanceController(
            IContextService contextService,
            IProductPerformanceService productService
            )
            : base(contextService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("api/performance/product/distribution")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDistribution([FromBody]ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetDistributionWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/diffences")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetDiffences([FromBody]ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetMeasurementDifferenceWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/period-growth")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetPeriodGrowth([FromBody]ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetPeriodGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product/month-growth-all")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetAllMonthGrowth([FromBody]ProductPerformanceFilterMetaModel filter)
        {
            var result = await _productService.GetAllDetailMonthGrowthWithComparison(filter);
            return ToResult(result);
        }

        [HttpPost]
        [Route("api/performance/product")]
        [AutoBindMetaModel]
        public async Task<JsonResult> GetCombineData([FromBody]ProductPerformanceFilterMetaModel filter)
        {
            await BindTopDistributionsToFilterMetaModel(filter);

            var distributions = null as Task<IEnumerable<DistributionVM>>;
            var overallMargin = null as Task<IEnumerable<OverallMeasurementVM>>;

            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                overallMargin = _productService.GetOverallMeasurement(filter);
            }
            else
            {
                distributions = _productService.GetDistributionWithComparison(filter);
            }

            var taskMeasurementDifference = _productService.GetMeasurementDifferenceWithComparison(filter);
            var taskPeriodGrowth = _productService.GetPeriodGrowthWithComparison(filter);

            return ToResult(new
            {
                BaseCurrency = filter.BaseCurrencyName,
                MeasurementId = filter.Measurement.Id,
                filter.CurrentPeriod,
                filter.PreviousPeriod,
                filter.TopDistributions,
                DistributionData = distributions != null ? await distributions : null,
                OverallMarginData = overallMargin != null ? await overallMargin : null,
                MeasurementDifferenceData = await taskMeasurementDifference,
                GrowthData = new
                {
                    PeriodGrowth = await taskPeriodGrowth,
                    MonthGrowth = new { }
                }
            });
        }

        private async Task BindTopDistributionsToFilterMetaModel(PerformanceFilterMetaModel filter)
        {
            var topDistributions = await _productService.GetTopDistributions(filter);
            filter.TopDistributions = topDistributions.Select(_ => new DistributionMetaModel
            {
                DisplayId = _.topDistributionId,
                DisplayName = _.topDistributionName
            });
        }
    }
}