using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Criteria
{
    public abstract class AnalyticCriteria : DAXQueryCriteria
    {
        public int SiteId { get; set; }
        public int BaseCurrency { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Measurement { get; set; }
        public int Perspective { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}