using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Repositories
{
    public interface IBaseRepo
    {
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null)
                        where TResult : new();

        Task<IEnumerable<TResult>> QueryAsync<TResult>(ISsasCriteria criteria)
                        where TResult : new();
    }
}