namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Repository;

    public interface IBaseRepository
    {
        Task<IEnumerable<TResult>> QueryAsync<TResult>(ICriteria criteria)
            where TResult : new();

        Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(params ICriteria[] listCriteria)
            where TResult : new();
    }
}