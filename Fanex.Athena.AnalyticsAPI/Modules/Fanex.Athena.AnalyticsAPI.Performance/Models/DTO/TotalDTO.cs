using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TotalDTO : IMeasurement
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }
    }
}