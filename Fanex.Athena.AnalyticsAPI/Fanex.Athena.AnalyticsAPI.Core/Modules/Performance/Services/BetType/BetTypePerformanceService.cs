using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public class BetTypePerformanceService : PerformanceService, IBetTypePerformanceService
    {
        public BetTypePerformanceService(IContextService context)
            : base(context)
        {

        }
    }
}