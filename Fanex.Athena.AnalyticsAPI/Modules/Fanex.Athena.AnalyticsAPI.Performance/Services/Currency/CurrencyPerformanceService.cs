using Fanex.Athena.AnalyticsAPI.Core.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class CurrencyPerformanceService : PerformanceService, ICurrencyPerformanceService
    {
        public CurrencyPerformanceService(IContextService context)
            : base(context)
        {
        }
    }
}