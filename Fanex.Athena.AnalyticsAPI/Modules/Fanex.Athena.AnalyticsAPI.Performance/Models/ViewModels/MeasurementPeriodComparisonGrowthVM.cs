using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class MeasurementPeriodComparisonGrowthVM
    {
        public string Name { get; set; }
        public double CurrentPeriodTotal { get; set; }
        public double PreviousPeriodTotal { get; set; }
        public double GrowthPercent { get; set; }
    }
}