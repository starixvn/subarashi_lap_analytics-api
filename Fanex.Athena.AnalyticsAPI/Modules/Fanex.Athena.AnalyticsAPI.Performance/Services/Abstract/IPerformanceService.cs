using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IPerformanceService
    {
        Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter, int numberOfTop = 5);
    }
}