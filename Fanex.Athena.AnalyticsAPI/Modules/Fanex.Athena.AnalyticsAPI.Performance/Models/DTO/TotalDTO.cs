namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    using Common.Attributes;

    public class TotalDTO : IMeasurement
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }
}