namespace Fanex.Athena.AnalyticsAPI.Performance.Helpers
{
    using System.Linq;
    using Common.Models.Criteria;
    using Models.Criteria;
    using Models.MetaModels;

    public static class CriteriaHelper
    {
        public static TCriteria ToPerformanceCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : AnalyticCriteria, new()
            where TMetaModel : PerformanceFilterMetaModel
        {
            var criteria = Common.Helpers.CriteriaHelper.ToCriteria<TCriteria, TMetaModel>(metaModel, isCurrentPeriod);
            if (criteria is IMeasurementCriteria measurementCriteria)
            {
                measurementCriteria.MeasurementId = metaModel.Measurement.Id;
            }

            return criteria;
        }

        public static TCriteria ToCurrencyPerformanceCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : CurrencyPerformanceCriteria, new()
            where TMetaModel : CurrenyPerformanceFilterMetaModel
        {
            var criteria = ToPerformanceCriteria<TCriteria, TMetaModel>(metaModel, isCurrentPeriod);
            criteria.ListCurrency = string.Join(',', metaModel.Currencies ?? Enumerable.Empty<string>());

            return criteria;
        }

        public static TCriteria ToProductPerformanceCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : ProductPerformanceCriteria, new()
            where TMetaModel : ProductPerformanceFilterMetaModel
        {
            var criteria = ToPerformanceCriteria<TCriteria, TMetaModel>(metaModel, isCurrentPeriod);
            criteria.ListProduct = string.Join(',', metaModel.Products?.Select(_ => _) ?? Enumerable.Empty<string>());

            return criteria;
        }

        public static TCriteria ToPlatformPerformanceCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : PlatformPerformanceCriteria, new()
            where TMetaModel : PlatformPerformanceFilterMetaModel
        {
            var criteria = ToPerformanceCriteria<TCriteria, TMetaModel>(metaModel, isCurrentPeriod);
            criteria.ListPlatform = string.Join(',', metaModel.Platforms?.Select(_ => _) ?? Enumerable.Empty<string>());

            return criteria;
        }

        public static TCriteria ToBettypePerformanceCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : BettypePerformanceCriteria, new()
            where TMetaModel : BettypePerformanceFilterMetaModel
        {
            var criteria = ToPerformanceCriteria<TCriteria, TMetaModel>(metaModel, isCurrentPeriod);
            criteria.ListCombineBettype = string.Join(',', metaModel.CombineIds?.Select(_ => _) ?? Enumerable.Empty<string>());

            return criteria;
        }
    }
}