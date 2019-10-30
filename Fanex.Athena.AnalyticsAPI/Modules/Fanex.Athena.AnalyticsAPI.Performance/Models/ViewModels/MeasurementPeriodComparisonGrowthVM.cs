using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementPeriodComparisonGrowthVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double CurrentPeriodTotal { get; set; }
        public double PreviousPeriodTotal { get; set; }
        public double GrowthPercent { get; set; }
        public IEnumerable<IdentifyVM> CurrentPeriodIdentifies { get; set; }
        public IEnumerable<IdentifyVM> PreviousPeriodIdentifies { get; set; }
    }

    public class IdentifyVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}