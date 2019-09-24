using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class TopChangeVM
    {
        public IEnumerable<OverviewTopChangeResultItemVM> Table { get; set; }
        public int Perspective { get; set; }
    }

    public class OverviewTopChangeResultItemVM
    {
        public decimal? Total { get; set; }
        public double? PercentChange { get; set; }
    }
}