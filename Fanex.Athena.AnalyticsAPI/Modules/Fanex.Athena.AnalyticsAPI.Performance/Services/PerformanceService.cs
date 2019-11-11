using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService : IPerformanceService
    {
        protected readonly IContextService _context;

        protected readonly IPerformanceRepository _performanceRepository;

        public PerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository
            )
        {
            _context = context;
            _performanceRepository = performanceRepository;
        }

        public async Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter)
        {
            var distributionData = await GetDistributionData(filter);
            var result = GetTopDistributions(distributionData);
            return result;
        }

        protected virtual Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            throw new NotImplementedException(nameof(GetDistributionData));
        }

        protected static IEnumerable<(string topDistributionId, string topDistributionName)> GetTopDistributions<T>(IEnumerable<T> dtos, int numberOfTop = 5)
            where T : DistributionDTO
        {
            dtos = dtos.Where(_ => !_.IsSum);
            var numberOfElement = dtos.Count();
            var sortedByDistribution = dtos.FirstOrDefault(_ => !_.Distribution.Equals(default)) != null;
            dtos = sortedByDistribution ? dtos.Take(numberOfTop) : dtos.OrderByDescending(_ => Math.Abs(_.Total)).Take(numberOfTop);

            var topDistributions = dtos.Select(_ => (_.Id, _.Name));

            if (numberOfElement > numberOfTop)
            {
                topDistributions = topDistributions.Append(("0", "Others"));
            }

            return topDistributions;
        }

        protected static string GetFieldNameOfMonthGrowthObject(string name, MonthRangeMetaModel period)
        {
            return $"{name}_{period.FromMonth}_{period.ToMonth}";
        }

        protected static IEnumerable<(string topDistributionId, string topDistributionName)> MapToTopDistributions(IEnumerable<DistributionMetaModel> topDistributions)
        {
            return topDistributions?.Select(_ => (_.DisplayId, _.DisplayName))
                ?? new List<(string topDistributionId, string topDistributionName)>();
        }
    }
}