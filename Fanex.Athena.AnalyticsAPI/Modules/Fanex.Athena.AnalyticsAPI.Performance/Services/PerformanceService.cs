using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService : BaseService, IPerformanceService
    {
        public PerformanceService(
            IContextService context,
            ISsasService ssasService,
            IDAXQueryRepository daxQueryRepo)
            : base(context, ssasService, daxQueryRepo)
        {
        }

        protected override string ConnectionKey => "Performance";
    }
}