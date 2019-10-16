using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class DistributionDTO
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }

    public class CurrencyDistributionDTO : DistributionDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public string CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        [SsasColumn("[IsTotal]")]
        public bool IsTotal { get; set; }
    }
}