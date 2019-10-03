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
        public BridgeSetting Setting { get; set; }

        public SsasBridgeService(string connectionKey, string bridgeHost)
        {
            Setting = new BridgeSetting
            {
                ConnectionKey = connectionKey,
                BridgeHost = bridgeHost
            };
        }

        public SsasBridgeService()
        {
            Setting = new BridgeSetting();
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string query, IDictionary<string, object> parameters = null, string bridgeUrl = "ssas/query")
            where TResult : new()
        {
            var result = Activator.CreateInstance<List<TResult>>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Setting.BridgeHost);
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, bridgeUrl);
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { query, connectionKey = Setting.ConnectionKey, parameters }), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(requestMessage);
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
                else
                {
                    throw new Exception(content);
                }
            }

            return result;
        }
    }
}