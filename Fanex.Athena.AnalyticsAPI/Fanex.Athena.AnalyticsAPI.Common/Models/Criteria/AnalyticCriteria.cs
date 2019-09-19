using Fanex.Athena.LGAPI.SSAS.Bridge.Utils;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Criteria
{
    public class AnalyticCriteria : BaseSsasCriteria
    {
        public int SiteId { get; set; }
        public int BaseCurrency { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}