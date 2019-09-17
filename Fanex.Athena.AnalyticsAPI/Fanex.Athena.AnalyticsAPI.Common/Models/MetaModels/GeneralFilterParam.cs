using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class GeneralFilterParam : BaseParam
    {
        [ParamQueryMapping("siteId")]
        public int LicenseeId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int BaseCurrency { get; set; }

        [ParamQueryMapping(NoneParam = true)]
        public ComparisonPeriodParam ComparisonPeriod { get; set; }
    }
}