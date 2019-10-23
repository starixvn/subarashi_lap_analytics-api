using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.DTO
{
    public abstract class DistributionDTO : IIdentifiable
    {
        [SsasColumn("[Measurement]")]
        public double Total { get; set; }

        [SsasColumn("[IsTotal]")]
        public bool IsSum { get; set; }

        public abstract string Id { get; }
        public abstract string Name { get; }
    }
}