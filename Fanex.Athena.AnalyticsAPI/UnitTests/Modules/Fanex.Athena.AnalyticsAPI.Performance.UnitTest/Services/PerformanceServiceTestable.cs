using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class PerformanceServiceTestable : PerformanceService
    {
        private PerformanceServiceTestable(
            IContextService context,
            IPerformanceRepository performanceRepository
        )
          : base(context, performanceRepository)
        {
        }
    }
}