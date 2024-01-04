using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class TopChangeVM
    {
        public string PerspectiveId { get; set; }

        [Translation(nameof(PerspectiveId))]
        public string PerspectiveName { get; set; }

        public double Total { get; set; }

        public double ChangePercent { get; set; }
    }
}