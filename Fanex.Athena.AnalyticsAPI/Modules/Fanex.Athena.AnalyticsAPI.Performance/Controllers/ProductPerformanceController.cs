using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class ProductPerformanceController : ControllerBaseCustom
    {
        private readonly IProductPerformanceService _productPerformanceService;

        public ProductPerformanceController(IProductPerformanceService productPerformanceService) : base(null)
        {
            _productPerformanceService = productPerformanceService;
        }

        [HttpPost]
        [Route("performance/product/breakdown")]
        public async Task<IActionResult> GetTotal(GeneralFilterParam filterParam)
        {
            var result = await this._productPerformanceService.GetBreakdown(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/product/growth")]
        public async Task<IActionResult> GetGrowth(GeneralFilterParam filterParam)
        {
            var result = await this._productPerformanceService.GetGrowth(filterParam);
            return ToResult(result);
        }

        [HttpPost]
        [Route("performance/product/distribution")]
        public async Task<IActionResult> GetDistribution(GeneralFilterParam filterParam)
        {
            var result = await this._productPerformanceService.GetDistribution(filterParam);
            return ToResult(result);
        }
    }
}