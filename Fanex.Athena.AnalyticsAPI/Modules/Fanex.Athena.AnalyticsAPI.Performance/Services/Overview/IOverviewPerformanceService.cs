using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IOverviewPerformanceService : IPerformanceService
    {
        Task<IEnumerable<OverviewTotalResultVM>> GetTotalResultWithComparison(PerformanceFilterMetalModel filter);

        Task<IEnumerable<string>> GetTopChangeResultWithComparison(PerformanceFilterMetalModel filter);

        Task<IEnumerable<string>> GetGrowthResult(PerformanceFilterMetalModel filter);
    }
}