using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IPerformanceService
    {
        Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter, int numberOfTop = 5);
    }
}