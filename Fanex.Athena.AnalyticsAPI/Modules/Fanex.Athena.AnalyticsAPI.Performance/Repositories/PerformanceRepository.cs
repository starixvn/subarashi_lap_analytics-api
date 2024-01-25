using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    public class PerformanceRepository : BaseRepository, IPerformanceRepository
    {
        public PerformanceRepository(ISqlService sqlService) : base(sqlService)
        {
        }

        protected override string ModuleId => "Performance";
    }
}