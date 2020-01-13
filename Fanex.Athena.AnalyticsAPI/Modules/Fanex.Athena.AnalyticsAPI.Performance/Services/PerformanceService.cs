using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Constants;
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
        private static readonly IHandleData MARGIN_HANDLER = new MarginHandler();

        protected const int NUMBER_OF_TOP_DISTRIBUTION = 5;

        protected readonly IContextService _context;
        protected readonly IPerformanceRepository _performanceRepository;
        protected readonly IHandleRawData _handleRawData;

        public PerformanceService(
            IContextService context,
            IPerformanceRepository performanceRepository,
            IHandleRawData handleRawData
            )
        {
            _context = context;
            _performanceRepository = performanceRepository;
            _handleRawData = handleRawData;
        }

        public async Task<IEnumerable<(string topDistributionId, string topDistributionName)>> GetTopDistributions(PerformanceFilterMetaModel filter, int numberOfTop = 5)
        {
            var distributionData = await GetDistributionData(filter);
            var result = GetTopDistributions(distributionData, numberOfTop);
            return result;
        }

        protected void HandleRawData<T>(IEnumerable<T> listData, PerformanceFilterMetaModel filter)
        {
            var handlers = new List<IHandleData>();
            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                handlers.Add(MARGIN_HANDLER);
            }
            _handleRawData.HandleRawData(listData, handlers);
        }

        protected void HandleRawData<T>(IEnumerable<IEnumerable<T>> listData, PerformanceFilterMetaModel filter)
        {
            var handlers = new List<IHandleData>();
            if (filter.Measurement.Id == MeasurementConst.MARGIN)
            {
                handlers.Add(MARGIN_HANDLER);
            }
            _handleRawData.HandleRawData(listData, handlers);
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
    }
}