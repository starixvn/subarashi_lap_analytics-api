using Fanex.Athena.AnalyticsAPI.Common.Services;

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