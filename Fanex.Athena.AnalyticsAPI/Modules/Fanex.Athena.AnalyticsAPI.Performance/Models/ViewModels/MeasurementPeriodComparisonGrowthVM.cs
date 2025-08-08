namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    using System.Collections.Generic;

    public class MeasurementPeriodComparisonGrowthVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double CurrentPeriodTotal { get; set; }

        public double PreviousPeriodTotal { get; set; }

        public double GrowthPercent { get; set; }

        public IEnumerable<IdentifyVM> CurrentPeriodDistributions { get; set; }

        public IEnumerable<IdentifyVM> PreviousPeriodDistributions { get; set; }
    }
}