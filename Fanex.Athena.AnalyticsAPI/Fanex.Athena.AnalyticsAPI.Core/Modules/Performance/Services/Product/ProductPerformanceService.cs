using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public class ProductPerformanceService : PerformanceService, IProductPerformanceService
    {
        public ProductPerformanceService(IContextService context)
            : base(context)
        {

        }
    }
}