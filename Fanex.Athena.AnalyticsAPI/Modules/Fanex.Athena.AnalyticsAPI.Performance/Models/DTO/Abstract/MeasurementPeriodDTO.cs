namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public abstract class MeasurementPeriodDTO : IIdentifiable, IMeasurement
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }
    }
}