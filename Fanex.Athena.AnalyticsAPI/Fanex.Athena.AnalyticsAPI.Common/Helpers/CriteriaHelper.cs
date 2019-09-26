using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public static class CriteriaHelper
    {
        public static TCriteria ToCriteria<TCriteria, TMetalModel>(TMetalModel metaModel, bool isCurrentPeriod = true)
            where TCriteria : AnalyticCriteria, new()
            where TMetalModel : GeneralFilterMetaModel
        {
            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;

            var criteria = new TCriteria
            {
                BaseCurrencyId = metaModel.BaseCurrency.Id,
                SiteName = metaModel.SiteName,
                FromDate = period.FromMonth,
                ToDate = period.ToMonth
            };

            return criteria;
        }
    }
}