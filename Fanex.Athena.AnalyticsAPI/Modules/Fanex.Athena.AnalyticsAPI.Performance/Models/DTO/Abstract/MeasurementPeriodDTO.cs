using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public abstract class MeasurementPeriodDTO : IIdentifiable, IMeasurement
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }
    }
}