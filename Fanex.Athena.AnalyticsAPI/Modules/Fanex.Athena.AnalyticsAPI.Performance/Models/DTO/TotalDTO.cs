using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public class TotalDTO
    {
        [SsasColumn("[Measurement]")]
        public decimal Total { get; set; }
    }
}