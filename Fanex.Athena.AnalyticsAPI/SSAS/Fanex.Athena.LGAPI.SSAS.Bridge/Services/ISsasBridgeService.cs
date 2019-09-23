using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public interface ISsasBridgeService
    {
        BridgeSetting Setting { get; set; }

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null, string bridgeUrl = "ssas/query")
                        where TResult : new();
    }
}