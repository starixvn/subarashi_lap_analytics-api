using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services.SqlService
{
    public interface ISqlService
    {
        Task<IEnumerable<TResult>> QueryAsync<TResult>(object parameterSet);
    }
}