using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Helpers
{
    public static class CriteriaHelper
    {
        public static TCriteria ToPerformanceCriteria<TCriteria, TMetalModel>(TMetalModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : AnalyticCriteria, new()
            where TMetalModel : PerformanceFilterMetaModel
        {
            var criteria = Common.Helpers.CriteriaHelper.ToCriteria<TCriteria, TMetalModel>(metaModel, isCurrentPeriod);
            if (criteria is IMeasurementCriteria measurementCriteria)
            {
                measurementCriteria.MeasurementId = metaModel.Measurement.Id;
            }

            return criteria;
        }

        public static TCriteria ToCurrencyPerformanceCriteria<TCriteria, TMetalModel>(TMetalModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : CurrencyPerformanceCriteria, new()
            where TMetalModel : CurrenyPerformanceFilterMetalModel
        {
            var criteria = ToPerformanceCriteria<TCriteria, TMetalModel>(metaModel, isCurrentPeriod);
            criteria.ListCurrency = string.Join(',', metaModel.Currencies ?? Enumerable.Empty<string>());

            return criteria;
        }
    }
}