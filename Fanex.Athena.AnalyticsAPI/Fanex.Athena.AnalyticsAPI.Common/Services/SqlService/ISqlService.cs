using System;
using System.Data;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ISqlService
    {
        Task<object> ExecuteNonQueryWithOutput(ICriteria criteria, string outputName);

        Task<T> ExecuteQueryWithReadHandler<T>(string spKey, object spParams, Func<IDataReader, T> readHandler);
    }
}