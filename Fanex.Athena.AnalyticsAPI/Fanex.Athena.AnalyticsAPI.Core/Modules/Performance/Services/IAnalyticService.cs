using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public interface IAnalyticService : IBaseService
    {
        Task<TotalAnalyticResult> GetTotal();

        Task<GrowthAnalyticResult> GetGrowth();

        Task<DistributionAnalyticResult> GetDistribution();
    }
}