namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.MetaModels;

    public interface IPerformanceService
    {
        Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter, int numberOfTop = 5);
    }
}