using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService : BaseService, IPerformanceService
    {
        protected readonly IPerformanceRepo _performanceRepo;

        public PerformanceService(IContextService context, IPerformanceRepo performanceRepo)
            : base(context)
        {
            _performanceRepo = performanceRepo;
        }
    }
}