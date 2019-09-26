using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Helpers
{
    public static class CriteriaHelper
    {
        public static TCriteria ToCriteria<TCriteria, TMetalModel>(TMetalModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : AnalyticCriteria, IMeasurementCriteria, new()
            where TMetalModel : PerformanceFilterMetaModel
        {
            var criteria = Common.Helpers.CriteriaHelper.ToCriteria<TCriteria, TMetalModel>(metaModel, isCurrentPeriod);
            criteria.MeasurementId = metaModel.Measurement.Id;

            return criteria;
        }
    }
}