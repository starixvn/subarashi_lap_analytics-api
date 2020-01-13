using System.Threading.Tasks;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Services
{
    public interface IHttpClientService
    {
        Task<TResult> PostAsync<TResult>(string url, object body);
    }
}