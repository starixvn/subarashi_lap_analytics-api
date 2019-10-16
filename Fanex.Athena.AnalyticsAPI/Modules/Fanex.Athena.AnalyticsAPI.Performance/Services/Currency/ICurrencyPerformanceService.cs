using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface ICurrencyPerformanceService
    {
        Task<IEnumerable<DistributionVM>> GetDistributionWithComparison(PerformanceFilterMetaModel filter);
    }
}