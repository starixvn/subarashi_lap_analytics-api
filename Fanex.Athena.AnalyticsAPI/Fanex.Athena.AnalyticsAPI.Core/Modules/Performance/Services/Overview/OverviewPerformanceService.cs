using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public class OverviewPerformanceService : PerformanceService, IOverviewPerformanceService
    {
        public OverviewPerformanceService(IContextService context)
            : base(context)
        {

        }
    }
}