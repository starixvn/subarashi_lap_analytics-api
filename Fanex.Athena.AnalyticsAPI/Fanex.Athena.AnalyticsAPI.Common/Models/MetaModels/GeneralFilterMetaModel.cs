using Fanex.Athena.AnalyticsAPI.Common.Helpers;
using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterMetaModel : IDateRangeCompare
    {
        private string _fromDate;
        public string FromDate
        {
            get
            {
                return DateTimeHelper.ParseToFullDateString(_fromDate, false);
            }
            set
            {
                _fromDate = value;
            }
        }

        private string _toDate;
        public string ToDate
        {
            get
            {
                return DateTimeHelper.ParseToFullDateString(_toDate, true);
            }
            set
            {
                _toDate = value;
            }
        }

        public SelectMetaModel BaseCurrency { get; set; }

        public SelectMetaModel Measurement { get; set; }

        public ComparisonPeriodMetaModel ComparePeriod { get; set; }

        public virtual T ToCriteria<T>()
            where T : AnalyticCriteria
        {
            var criteria = Activator.CreateInstance<T>();
            criteria.BaseCurrencyId = BaseCurrency.Id;
            criteria.SiteName = "12BET";
            criteria.FromDate = FromDate;
            criteria.ToDate = ToDate;

            return criteria;
        }

        public void UpdateDateRangeByComparePeriod()
        {
            var from = DateTimeHelper.ParseToFullDate(_fromDate, false);
            var to = DateTimeHelper.ParseToFullDate(_toDate, false);

            if (ComparePeriod.IsPreviousPeriod == true)
            {
                var monthDistance = (12 + (to.Month - from.Month)) % 12;
                ToDate = from.AddMonths(-1).ToString(DateTimeHelper.MONTH_FORMAT);
                FromDate = from.AddMonths(-(monthDistance + 1)).ToString(DateTimeHelper.MONTH_FORMAT);
            }
            else if(ComparePeriod.SamePeriodOfYear.HasValue)
            {
                var selectedYear = ComparePeriod.SamePeriodOfYear.Value;

                FromDate = new DateTime(selectedYear - (to.Year - from.Year), from.Month, 1)
                    .ToString(DateTimeHelper.MONTH_FORMAT);
                ToDate = new DateTime(selectedYear, to.Month, 1)
                    .ToString(DateTimeHelper.MONTH_FORMAT);
            }
            else
            {
                throw new Exception($"{nameof(ComparePeriod)} is invalid");
            }
        }
    }
}