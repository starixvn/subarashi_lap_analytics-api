using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ISqlService
    {
        Task<T> ExecuteQueryWithReadHandler<T>(string spKey, object spParams, Func<IDataReader, T> readHandler);

        Task<IEnumerable<T>> ExecuteQueryData<T>(ICriteria criteria) where T : new();

        Task<IEnumerable<IEnumerable<T>>> ExecuteMultipleQueryData<T>(params ICriteria[] listCriteria) where T : new();
    }
}