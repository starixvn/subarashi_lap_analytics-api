using Fanex.Athena.AnalyticsAPI.Core.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class ProductPerformanceService : PerformanceService, IProductPerformanceService
    {
        public ProductPerformanceService(IContextService context)
            : base(context)
        {
        }
    }
}