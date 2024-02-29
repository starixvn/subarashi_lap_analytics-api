using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Repositories
{
    public class SportsbookRepository : BaseRepository, ISportsbookRepository
    {
        public SportsbookRepository(ISqlService sqlService) : base(sqlService)
        {
        }

        protected override string ModuleId => "Sportsbook";
    }
}