using Fanex.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services.SqlService
{
    public class SqlService : ObjectDb
    {
        public SqlService(string key, int? commandTimeout = null)
            : base(key, commandTimeout, null)
        {
        }

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(object parameterSet)
        {
            return base.QueryAsync<TResult>(parameterSet);
        }

        public Task<TResult> QueryEntityAsync<TResult>(object parameterSet)
        {
            return base.QueryEntityAsync<TResult>(parameterSet);
        }
    }
}