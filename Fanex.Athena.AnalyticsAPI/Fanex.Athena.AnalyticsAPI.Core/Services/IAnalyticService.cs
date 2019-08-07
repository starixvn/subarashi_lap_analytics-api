using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IAnalyticService<TTotal, TGrowth, TDistribution> : IBaseService
    {
        Task<TDistribution> GetDistribution(GeneralFilterParam filterParam);

        Task<TGrowth> GetGrowth(GeneralFilterParam filterParam);

        Task<TTotal> GetTotal(GeneralFilterParam filterParam);
    }
}