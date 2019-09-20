using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public interface IDAXQueryRepository
    {
        string GetQuery(DAXQueryCriteria criteria);
    }
}