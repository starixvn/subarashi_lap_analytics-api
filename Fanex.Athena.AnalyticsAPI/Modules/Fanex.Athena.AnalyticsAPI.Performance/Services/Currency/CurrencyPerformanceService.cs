using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class CurrencyPerformanceService : PerformanceService, ICurrencyPerformanceService
    {
        public CurrencyPerformanceService(
            IContextService context,
            ISsasService ssasService,
            IDAXQueryRepository daxQueryRepo)
            : base(context, ssasService, daxQueryRepo)
        {
        }
    }
}