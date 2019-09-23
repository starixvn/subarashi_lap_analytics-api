using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService
    {
        protected readonly IContextService _context;

        protected readonly IPerformanceRepository _performanceRepository;

        public PerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository
            )
        {
            _context = context;
            _performanceRepository = performanceRepository;
        }
    }
}