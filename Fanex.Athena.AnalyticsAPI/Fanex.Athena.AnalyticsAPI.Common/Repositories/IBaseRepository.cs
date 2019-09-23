using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public interface IBaseRepository
    {
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null)
            where TResult : new();

        Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new();
    }
}