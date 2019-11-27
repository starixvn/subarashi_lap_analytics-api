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
        protected const int NUMBER_OF_TOP_DISTRIBUTION = 5;

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

        public async Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter, int numberOfTop = 5)
        {
            var distributionData = await GetDistributionData(filter);
            var result = GetTopDistributions(distributionData, numberOfTop);
            return result;
        }

        protected virtual Task<IEnumerable<DistributionDTO>> GetDistributionData(PerformanceFilterMetaModel filter)
        {
            throw new NotImplementedException(nameof(GetDistributionData));
        }

        protected static IEnumerable<(string topDistributionId, string topDistributionName)> GetTopDistributions<T>(IEnumerable<T> elements, int numberOfTop)
            where T : DistributionDTO
        {
            elements = elements.Where(_ => !_.IsSum);
            var topDistributions = elements
                .OrderByDescending(_ => Math.Abs(_.Distribution))
                .Take(numberOfTop)
                .Select(_ => (_.Id, _.Name));

            if (elements.Count() > numberOfTop)
            {
                topDistributions = topDistributions.Append(("0", "Others"));
            }

            return topDistributions;
        }

        protected static string GetFieldNameOfMonthGrowthObject(string key, MonthRangeMetaModel period)
        {
            return $"{key}_{period.FromMonth}_{period.ToMonth}";
        }

        protected static IEnumerable<(string topDistributionId, string topDistributionName)> MapToTopDistributions(IEnumerable<DistributionMetaModel> topDistributions)
        {
            return topDistributions?.Select(_ => (_.DisplayId, _.DisplayName))
                ?? new List<(string topDistributionId, string topDistributionName)>();
        }
    }
}