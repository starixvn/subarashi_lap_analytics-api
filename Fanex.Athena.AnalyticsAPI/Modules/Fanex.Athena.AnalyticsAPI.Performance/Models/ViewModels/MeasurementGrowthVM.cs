using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementGrowthVM
    {
        public string Id { get; set; }

        [Translation(nameof(Id))]
        public string Name { get; set; }

        public double Total { get; set; }

        public double GrowthPercent { get; set; }
    }
}