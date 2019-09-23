using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Criteria
{
    public class AnalyticCriteria : ICriteria
    {
        protected AnalyticCriteria()
        {
        }

        public string SiteName { get; set; }
        public int BaseCurrencyId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public virtual string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool IsValid()
        {
            return true;
        }
    }
}