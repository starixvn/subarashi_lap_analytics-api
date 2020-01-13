using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.LGAPI.SSAS.Bridge.Services;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class PerformanceRepository : BaseRepository, IPerformanceRepository
    {
        public PerformanceRepository(
            IOptions<SSASSetting> setting,
            IDaxQueryContentService daxQueryService,
            ISsasBridgeService bridgeService
            ) : base(setting, daxQueryService, bridgeService)
        {
        }

        protected override string ModuleId => "Performance";
    }
}