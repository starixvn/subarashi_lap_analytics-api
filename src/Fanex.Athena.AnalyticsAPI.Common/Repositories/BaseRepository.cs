namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Repository;
    using Services;

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