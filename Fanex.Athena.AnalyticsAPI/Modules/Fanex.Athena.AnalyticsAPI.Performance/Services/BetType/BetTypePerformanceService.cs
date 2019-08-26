using Fanex.Athena.AnalyticsAPI.Core.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class BetTypePerformanceService : PerformanceService, IBetTypePerformanceService
    {
        public BetTypePerformanceService(IContextService context)
            : base(context)
        {
        }
    }
}