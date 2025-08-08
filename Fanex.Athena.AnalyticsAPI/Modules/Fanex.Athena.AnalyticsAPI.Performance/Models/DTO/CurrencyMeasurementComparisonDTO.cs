namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public class CurrencyMeasurementComparisonDTO : MeasurementComparisonDTO
    {
        [SsasColumn("DimCurrency[CurrencyID]")]
        public string CurrencyId { get; set; }

        [SsasColumn("DimCurrency[CurrencyName]")]
        public string CurrencyName { get; set; }

        public override string Id => CurrencyId;

        public override string Name => CurrencyName;
    }
}