using Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Core.Services;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public interface IPerformanceService : IAnalyticService<PerformanceTotalResult, PerformanceGrowthResult, PerformanceDistributionResult>
    {
    }
}