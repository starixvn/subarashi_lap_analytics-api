namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    using Common.Attributes;

    public class TopChangeVM
    {
        public string PerspectiveId { get; set; }

        [Translation(nameof(PerspectiveId))]
        public string PerspectiveName { get; set; }

        public double Total { get; set; }

        public double ChangePercent { get; set; }
    }
}