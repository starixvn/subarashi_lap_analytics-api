using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TotalResultDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("[Licensee Turnover]")]
        public decimal Turnover { get; set; }

        [SsasColumn("[Licensee Winloss]")]
        public decimal GrossProfit { get; set; }

        [SsasColumn("[Licensee Margin]")]
        public double Margin { get; set; }
    }
}