using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public interface ISsasBridgeService
    {
        BridgeSetting Setting { get; set; }

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, string bridgeUrl = "SSAS/Query")
                        where TResult : new();

        Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(IEnumerable<string> queries, string bridgeUrl = "SSAS/QueryMultiple")
            where TResult : new();
    }
}