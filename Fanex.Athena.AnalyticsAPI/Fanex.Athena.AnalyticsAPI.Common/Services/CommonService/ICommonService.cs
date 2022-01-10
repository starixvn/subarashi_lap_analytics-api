using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICommonService
    {
        Task<BalanceUp> GetBalanceUp();
    }
}