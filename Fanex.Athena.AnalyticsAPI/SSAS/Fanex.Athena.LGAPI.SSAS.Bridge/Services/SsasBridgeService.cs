using Fanex.Athena.LGAPI.SSAS.Bridge.Extensions;
using Fanex.Athena.LGAPI.SSAS.Bridge.Helpers;
using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public class SsasBridgeService : ISsasBridgeService
    {
        private readonly HttpClient _client;

        public BridgeSetting Setting { get; set; }

        public SsasBridgeService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, string bridgeUrl = "SSAS/Query")
            where TResult : new()
        {
            var result = new List<TResult>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, bridgeUrl))
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    query,
                    connectionKey = Setting.ConnectionKey
                }), Encoding.UTF8, "application/json");

                using (var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var content = await response.Content.ReadAsStreamAsync())
                        {
                            var records = JsonHelper.DeserializeJsonFromStream<IEnumerable<IDictionary<string, object>>>(content);
                            result.Capacity = records.Count();
                            result.AddRange(ParseToObjects<TResult>(records));
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<IEnumerable<TResult>>> QueryMultipleAsync<TResult>(
            IEnumerable<string> queries,
            string bridgeUrl = "SSAS/QueryMultiple")
           where TResult : new()
        {
            var result = new List<IEnumerable<TResult>>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, bridgeUrl))
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    queries,
                    connectionKey = Setting.ConnectionKey,
                }), Encoding.UTF8, "application/json");

                using (var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var content = await response.Content.ReadAsStreamAsync())
                        {
                            var items = JsonHelper.DeserializeJsonFromStream<IEnumerable<IEnumerable<IDictionary<string, object>>>>(content);
                            foreach (var item in items)
                            {
                                result.Add(ParseToObjects<TResult>(item));
                            }
                        }
                    }
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