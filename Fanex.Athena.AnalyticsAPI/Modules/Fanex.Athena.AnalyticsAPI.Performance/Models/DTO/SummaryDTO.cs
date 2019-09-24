using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class SummaryDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("[Turnover]")]
        public double Turnover { get; set; }

        [SsasColumn("[Gross Profit]")]
        public double GrossProfit { get; set; }

        [SsasColumn("[Margin]")]
        public double Margin { get; set; }
    }
}