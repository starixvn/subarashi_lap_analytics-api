using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class SummaryResultDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("[Turnover]")]
        public decimal Turnover { get; set; }

        [SsasColumn("[Gross Profit]")]
        public decimal GrossProfit { get; set; }

        [SsasColumn("[Margin]")]
        public double Margin { get; set; }
    }
}