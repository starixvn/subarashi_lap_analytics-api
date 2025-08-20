namespace Fanex.Athena.AnalyticsAPI.Performance.Repositories
{
    using Common.Repositories;
    using Common.Services;

    public class PerformanceRepository : BaseRepository, IPerformanceRepository
    {
        public PerformanceRepository(ISqlService sqlService) : base(sqlService)
        {
        }

        protected override string ModuleId => "Performance";
    }
}