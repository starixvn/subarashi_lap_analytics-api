using Fanex.Athena.LGAPI.SSAS.Bridge.Helpers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResult> PostAsync<TResult>(string url, object body)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                using (var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var streamContent = await response.Content.ReadAsStreamAsync())
                        {
                            return JsonHelper.DeserializeJsonFromStream<TResult>(streamContent);
                        }
                    }
                    return default(TResult);
                }
            }
        }
    }
}