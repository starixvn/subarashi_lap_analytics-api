using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IPerformanceService : IAnalyticService<PerformanceTotalResult, PerformanceGrowthResult, PerformanceDistributionResult>
    {
    }
}