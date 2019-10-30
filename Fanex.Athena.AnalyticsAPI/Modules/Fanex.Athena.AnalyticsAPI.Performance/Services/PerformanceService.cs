using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService
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

        protected static IEnumerable<(string topDistributionId, string topDistributionName)> GetTopDistributions<T>(IEnumerable<T> dtos, int numberOfTop = 5)
            where T : DistributionDTO
        {
            var topDistributions = dtos
                .Where(_ => !_.IsSum)
                .OrderByDescending(_ => Math.Abs(_.Total))
                .Take(numberOfTop)
                .Select(_ => (_.Id, _.Name));

            if (dtos.Count() > numberOfTop)
            {
                topDistributions = topDistributions.Append(("0", "Others"));
            }

            return topDistributions;
        }

        protected static string GetFieldNameOfMonthGrowthObject(string name, MonthRangeMetaModel period)
        {
            return $"{name}_{period.FromMonth}_{period.FromMonth}";
        }
    }
}