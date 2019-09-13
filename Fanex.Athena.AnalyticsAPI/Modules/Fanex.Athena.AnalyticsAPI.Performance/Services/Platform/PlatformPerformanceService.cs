using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PlatformPerformanceService : PerformanceService, IPlatformPerformanceService
    {
        public PlatformPerformanceService(IContextService context)
            : base(context)
        {
        }
    }
}