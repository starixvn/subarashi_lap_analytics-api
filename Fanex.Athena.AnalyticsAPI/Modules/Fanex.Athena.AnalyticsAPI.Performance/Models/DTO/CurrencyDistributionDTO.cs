using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public abstract class DistributionDTO
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }

        [SsasColumn("[IsTotal]")]
        public bool IsSum { get; set; }

        public abstract string GetName { get;}
    }

    public class CurrencyDistributionDTO : DistributionDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public string CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        public override string GetName => CurrencyName;
    }
}