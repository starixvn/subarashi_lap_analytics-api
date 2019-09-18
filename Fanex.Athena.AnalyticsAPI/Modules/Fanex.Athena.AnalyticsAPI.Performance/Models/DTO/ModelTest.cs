using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class ModelTest
    {
        [SsasColumn("DimSite[Licensee Name]")]
        public string LicenseeName { get; set; }

        [SsasColumn("DimCurrency[CurrencyID]")]
        public int CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        [SsasColumn("[Licensee Turnover]")]
        public decimal LicenseeTurnOver { get; set; }
    }
}