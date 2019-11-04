using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IAuthService
    {
        Task<string> Authenticate(string lapToken);
    }
}