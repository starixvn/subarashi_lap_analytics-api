using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly ISqlService _sqlService;

        protected BaseRepository(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new()
        {
            var result = await _sqlService.ExecuteQueryData<TResult>(criteria);

            return result;
        }

        public async Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(params ICriteria[] listCriteria)
            where TResult : new()
        {
            var result = await _sqlService.ExecuteMultipleQueryData<TResult>(listCriteria);

            return result;
        }

        protected abstract string ModuleId { get; }
    }
}