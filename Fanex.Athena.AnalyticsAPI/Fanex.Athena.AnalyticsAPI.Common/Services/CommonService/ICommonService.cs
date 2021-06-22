namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using Fanex.Athena.AnalyticsAPI.Common.Models.Configuration;
    using System.Threading.Tasks;

    public interface ICommonService
    {
        Task<BalanceUp> GetBalanceUp();
    }
}
