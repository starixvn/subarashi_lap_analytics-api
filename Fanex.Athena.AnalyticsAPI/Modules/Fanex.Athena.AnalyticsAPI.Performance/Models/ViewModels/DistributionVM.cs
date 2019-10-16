using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class DistributionVM
    {
        public string Name { get; set; }
        public IEnumerable<DistributionItemVM> Distributions { get; set; }
    }

    public class DistributionItemVM
    {
        public string Name { get; set; }
        public int Percent { get; set; }
    }
}