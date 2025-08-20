namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories
{
    using Common.Repositories;
    using Common.Services;

    public class SportsbookRepository : BaseRepository, ISportsbookRepository
    {
        public SportsbookRepository(ISqlService sqlService) : base(sqlService)
        {
        }

        protected override string ModuleId => "Sportsbook";
    }
}