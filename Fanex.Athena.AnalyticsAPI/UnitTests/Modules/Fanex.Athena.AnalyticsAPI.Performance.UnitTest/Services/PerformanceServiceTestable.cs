using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    public class DistributionDtoTestClass : DistributionDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }

    public class PerformanceServiceTestable : PerformanceService
    {
        public PerformanceServiceTestable(
            IContextService context,
            IPerformanceRepository performanceRepository,
            IHandleRawData handleRawData
        )
          : base(context, performanceRepository, handleRawData)
        {
        }

        public IEnumerable<(string topDistributionId, string topDistributionName)> GetTopDistributionsTestable<T>(IEnumerable<T> elements, int numberOfTop)
            where T : DistributionDTO
        {
            return GetTopDistributions(elements, numberOfTop);
        }

        public string GetFieldNameOfMonthGrowthObjectTestable(string key, MonthRangeMetaModel period)
        {
            return GetFieldNameOfMonthGrowthObject(key, period);
        }

        public IEnumerable<(string topDistributionId, string topDistributionName)> MapToTopDistributionsTestable(IEnumerable<DistributionMetaModel> topDistributions)
        {
            return MapToTopDistributions(topDistributions);
        }
    }
}