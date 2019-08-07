using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    [ApiController]
    public partial class PerformanceController : ControllerBaseCustom
    {
        private readonly IBetTypePerformanceService _betTypePerformanceService;
        private readonly ICurrencyPerformanceService _currencyPerformanceService;
        private readonly IOverviewPerformanceService _overviewPerformanceService;
        private readonly IPlatformPerformanceService _platformPerformanceService;
        private readonly IProductPerformanceService _productPerformanceService;

        public PerformanceController(
            IBetTypePerformanceService betTypePerformanceService,
            ICurrencyPerformanceService currencyPerformanceService,
            IOverviewPerformanceService overviewPerformanceService,
            IPlatformPerformanceService platformPerformanceService,
            IProductPerformanceService productPerformanceService
            )
        {
            _betTypePerformanceService = betTypePerformanceService;
            _currencyPerformanceService = currencyPerformanceService;
            _overviewPerformanceService = overviewPerformanceService;
            _platformPerformanceService = platformPerformanceService;
            _productPerformanceService = productPerformanceService;
        }
    }
}