namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    using Models.Criteria;
    using Models.MetaModels;

    public static class CriteriaHelper
    {
        public static TCriteria ToCriteria<TCriteria, TMetaModel>(TMetaModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : AnalyticCriteria, new()
            where TMetaModel : GeneralFilterMetaModel
        {
            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            var criteria = new TCriteria
            {
                BaseCurrencyName = CurrencyHelper.GetBasedCurrency(metaModel.BaseCurrencyName),
                SiteName = metaModel.SiteName,
                FromDate = period.FromMonth,
                ToDate = period.ToMonth
            };

            return criteria;
        }

        public static void UpdateCurrentPeriodToPreviousPeriod<TCriteria>(TCriteria criteria, MonthRangeMetaModel prevPeriod, MonthRangeMetaModel currentPeriod)
            where TCriteria : AnalyticCriteria, new()
        {
            var prevofPrevPeriod = MetaModelHelper.GetPrevOfPrevPeriod(prevPeriod, currentPeriod);
            criteria.FromDate = prevofPrevPeriod.FromMonth;
            criteria.ToDate = prevofPrevPeriod.ToMonth;
        }
    }
}