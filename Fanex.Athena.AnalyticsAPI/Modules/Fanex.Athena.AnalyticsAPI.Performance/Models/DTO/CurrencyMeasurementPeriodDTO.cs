using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class CurrencyMeasurementPeriodDTO : MeasurementPeriodDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public string CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        public override string Id => CurrencyId;

        public override string Name => CurrencyName;
    }
}