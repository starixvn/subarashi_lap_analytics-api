using Fanex.Athena.LGAPI.SSAS.Bridge.Extensions;
using Fanex.Athena.LGAPI.SSAS.Bridge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public class SsasBridgeService : ISsasBridgeService
    {
        private const string KEY_NAME = "LGAPI_TOKEN_KEY";

        private readonly HttpClient _client;

        private BridgeSetting _setting;

        public BridgeSetting Setting
        {
            get
            {
                return _setting;
            }
            set
            {
                _setting = value;
                if (_client != null)
                {
                    _client.BaseAddress = new Uri(_setting?.BridgeHost);
                    _client.DefaultRequestHeaders.Add(KEY_NAME, _setting?.AuthKey);
                }
            }
        }

        public SsasBridgeService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null, string bridgeUrl = "ssas/query")
            where TResult : new()
        {
            var result = Activator.CreateInstance<List<TResult>>();
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, bridgeUrl))
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { query, connectionKey = _setting.ConnectionKey, parameters }), Encoding.UTF8, "application/json");
                using (var response = await _client.SendAsync(requestMessage))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var records = JsonConvert.DeserializeObject<IEnumerable<IDictionary<string, object>>>(content);
                        result.Capacity = records.Count();
                        foreach (var record in records)
                        {
                            var obj = record.ToObject<TResult>();
                            result.Add(obj);
                        }
                    }
                }
            }            

            return result;
        }
    }
}