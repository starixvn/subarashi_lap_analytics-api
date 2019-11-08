using Fanex.Athena.LGAPI.SSAS.Bridge.Extensions;
using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public class SsasBridgeService : ISsasBridgeService
    {
        private readonly IHttpClientService _httpClientService;

        public BridgeSetting Setting { get; set; }

        public SsasBridgeService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, string bridgeUrl = "SSAS/Query")
            where TResult : new()
        {
            var body = new { query, connectionKey = Setting.ConnectionKey };
            var responseData = await _httpClientService.PostAsync<IEnumerable<IDictionary<string, object>>>(bridgeUrl, body);
            var result = responseData != null ? ParseToObjects<TResult>(responseData) : new List<TResult>();
            return result;
        }

        public async Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(IEnumerable<string> queries, string bridgeUrl = "SSAS/QueryMultiple")
            where TResult : new()
        {
            var body = new { queries, connectionKey = Setting.ConnectionKey };
            var responseData = await _httpClientService.PostAsync<IEnumerable<IEnumerable<IDictionary<string, object>>>>(bridgeUrl, body);
            var result = new List<IEnumerable<TResult>>();
            if (responseData != null)
            {
                foreach (var item in responseData)
                {
                    result.Add(ParseToObjects<TResult>(item));
                }
            }

            return result;
        }

        private static IEnumerable<TResult> ParseToObjects<TResult>(IEnumerable<IDictionary<string, object>> records)
            where TResult : new()
        {
            foreach (var record in records)
            {
                var obj = record.ToObject<TResult>();
                yield return obj;
            }
        }
    }
}