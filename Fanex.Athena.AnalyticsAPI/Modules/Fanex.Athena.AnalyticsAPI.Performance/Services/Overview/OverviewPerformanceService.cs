using Fanex.Athena.AnalyticsAPI.Core.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        public OverviewPerformanceService(IContextService context)
            : base(context)
        {
        }
    }
}