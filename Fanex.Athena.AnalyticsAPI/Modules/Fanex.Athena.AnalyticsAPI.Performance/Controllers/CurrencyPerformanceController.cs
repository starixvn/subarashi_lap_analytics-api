using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class CurrencyPerformanceController : ControllerBaseCustom
    {
        private readonly ICurrencyPerformanceService _currencyPerformanceService;

        public CurrencyPerformanceController(
            IContextService contextService,
            ICurrencyPerformanceService currencyPerformanceService
            )
            : base(contextService)
        {
            _currencyPerformanceService = currencyPerformanceService;
        }
    }
}