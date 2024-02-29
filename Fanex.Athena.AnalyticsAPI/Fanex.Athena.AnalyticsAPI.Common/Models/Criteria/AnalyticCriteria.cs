using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Criteria
{
    public abstract class AnalyticCriteria : CriteriaBase, ICriteria
    {
        public string SiteName { get; set; }

        public string BaseCurrencyName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public override bool IsValid()
        {
            return string.IsNullOrWhiteSpace(SiteName);
        }
    }
}