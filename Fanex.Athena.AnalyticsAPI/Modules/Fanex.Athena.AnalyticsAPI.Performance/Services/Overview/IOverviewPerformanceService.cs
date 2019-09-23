using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IOverviewPerformanceService
    {
        Task<IEnumerable<OverviewTotalResultVM>> GetSummaryResultWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<OverviewTopChangeResultVM>> GetTopChangeResultWithComparison(PerformanceFilterMetaModel filter);

        Task<IEnumerable<string>> GetGrowthResultWithComparison(PerformanceFilterMetaModel filter);
    }
}