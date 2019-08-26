using Fanex.Athena.AnalyticsAPI.Core.Controllers;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Performance.Controllers
{
    [ApiController]
    public class OverviewPerformanceController : ControllerBaseCustom
    {
        private readonly IOverviewPerformanceService _overviewPerformanceService;

        public OverviewPerformanceController(IOverviewPerformanceService overviewPerformanceService)
        {
            _overviewPerformanceService = overviewPerformanceService;
        }
    }
}