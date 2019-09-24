using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterMetaModel
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public SelectMetaModel BaseCurrency { get; set; }

        public SelectMetaModel Measurement { get; set; }

        public ComparisonPeriodMetaModel ComparisonPeriod { get; set; }

        public virtual TResult ToCriteria<TResult>()
            where TResult : AnalyticCriteria
        {
            var criteria = Activator.CreateInstance<TResult>();
            criteria.BaseCurrencyId = BaseCurrency.Id;
            criteria.SiteName = "12BET";
            criteria.FromDate = FromDate;
            criteria.ToDate = ToDate;
            return criteria;
        }

        public void UpdateDateRangeByComparisonPeriod()
        {
            //test mode
            if (ComparisonPeriod.IsPreviousPeriod)
            {
                FromDate = "05/01/2019";
                ToDate = "12/31/2019";
            }
            else
            {
                FromDate = "06/01/2019";
                ToDate = "12/31/2019";
            }
        }
    }
}