using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TopChangeResultDTO
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }
        [SsasColumn("DimCurrency[CurrencyName]")]
        public string Perspective { get; set; }
        [SsasColumn("[Total]")]
        public decimal Total { get; set; }
    }
}