using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICommonService
    {
        Task<BalanceUp> GetBalanceUp();
    }
}