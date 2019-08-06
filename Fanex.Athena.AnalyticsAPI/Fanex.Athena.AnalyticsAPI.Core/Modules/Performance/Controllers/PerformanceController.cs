using Fanex.Athena.AnalyticsAPI.Common.Controllers;
using Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services.BetType;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Controllers
{
    [ApiController]
    public partial class PerformanceController : ControllerBaseCustom
    {
        private readonly IBetTypePerformanceService _betTypePerformanceService;

        public PerformanceController(IBetTypePerformanceService betTypePerformanceService)
        {
            _betTypePerformanceService = betTypePerformanceService;
        }
    }
}