using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public interface IBaseRepository
    {
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query)
            where TResult : new();

        Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new();

        Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(IEnumerable<string> queries)
            where TResult : new();

        Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(params ICriteria[] listCriteria)
            where TResult : new();
    }
}