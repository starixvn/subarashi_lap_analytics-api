namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public class PlatformMeasurementComparisonDTO : MeasurementComparisonDTO
    {
        [SsasColumn("DimPlatForm[PlatformID]")]
        public string PlatformId { get; set; }

        [SsasColumn("DimPlatForm[Platform]")]
        public string PlatformName { get; set; }

        public override string Id => PlatformId;

        public override string Name => PlatformName;
    }
}