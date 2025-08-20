namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    using Common.Attributes;

    public class MeasurementGrowthVM
    {
        public string Id { get; set; }

        [Translation(nameof(Id))]
        public string Name { get; set; }

        public double Total { get; set; }

        public double GrowthPercent { get; set; }
    }
}