namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System.Threading.Tasks;
    using Models.Configuration;

    public interface ICommonService
    {
        Task<BalanceUp> GetBalanceUp();
    }
}