using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Services
{
    public interface IAnalyticService : IBaseService
    {
        Task<TotalResult> GetTotal();

        Task<GrowthResult> GetGrowth();

        Task<DistributionResult> GetDistribution();
    }
}