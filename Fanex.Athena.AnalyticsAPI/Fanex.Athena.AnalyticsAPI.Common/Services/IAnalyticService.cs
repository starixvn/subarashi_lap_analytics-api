using System.Threading.Tasks;

using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IAnalyticService<TTotal, TGrowth, TDistribution> : IBaseService
    {
        Task<TDistribution> GetDistribution(GeneralFilterParam filterParam);

        Task<TGrowth> GetGrowth(GeneralFilterParam filterParam);

        Task<TTotal> GetBreakdown(GeneralFilterParam filterParam);
    }
}