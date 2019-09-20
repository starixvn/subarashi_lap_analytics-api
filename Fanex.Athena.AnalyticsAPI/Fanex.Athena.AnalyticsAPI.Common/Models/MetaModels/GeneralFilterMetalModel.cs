using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterMetalModel
    {
        public int LicenseeId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public SelectMetalModel BaseCurrency { get; set; }

        public SelectMetalModel Measurement { get; set; }

        public ComparisonPeriodMetalModel ComparisonPeriod { get; set; }

        public virtual TResult ToCriteria<TResult>()
            where TResult : AnalyticCriteria
        {
            var criteria = Activator.CreateInstance<TResult>();
            criteria.BaseCurrency = BaseCurrency.Id;
            criteria.SiteId = LicenseeId;
            criteria.FromDate = FromDate;
            criteria.ToDate = ToDate;
            criteria.Measurement = Measurement.Id;

            return criteria;
        }
    }
}