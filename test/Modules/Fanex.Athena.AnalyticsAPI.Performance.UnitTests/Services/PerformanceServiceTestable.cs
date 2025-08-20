namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTests.Services
{
    using System.Collections.Generic;
    using Common.Models.MetaModels;
    using Common.Services;
    using Performance.Models.DTO;
    using Performance.Services;
    using Repositories;

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
    }
}