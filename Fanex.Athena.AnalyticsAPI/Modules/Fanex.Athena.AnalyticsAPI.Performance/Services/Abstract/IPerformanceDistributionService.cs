using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IPerformanceDistributionService
    {
        Task<(IEnumerable<DistributionDTO> prevPeriodData, IEnumerable<DistributionDTO> currentPeriodData)> GetDistributionRawData(PerformanceFilterMetaModel filter);
    }
}