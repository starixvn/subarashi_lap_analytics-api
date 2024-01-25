using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class PlatformMeasurementPeriodDTO : MeasurementPeriodDTO
    {
        [SsasColumn("DimPlatForm[PlatformID]")]
        public string PlatformId { get; set; }

        [SsasColumn("DimPlatForm[Platform]")]
        public string PlatformName { get; set; }

        public override string Id => PlatformId;

        public override string Name => PlatformName;
    }
}