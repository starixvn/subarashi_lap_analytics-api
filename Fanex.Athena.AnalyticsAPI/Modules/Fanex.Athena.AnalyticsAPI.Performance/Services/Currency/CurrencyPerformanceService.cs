using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class CurrencyPerformanceService : PerformanceService, ICurrencyPerformanceService
    {
        public CurrencyPerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository
            ) : base(context, performanceRepository)
        {
        }
    }
}