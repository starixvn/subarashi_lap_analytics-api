using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    public class DistributionVM
    {
        public DistributionVM(int sizeDistribution = 6)
        {
            Distributions = new List<DistributionItemVM>(sizeDistribution);
        }

        public string Name { get; set; }
        public double Total { get; set; }
        public IList<DistributionItemVM> Distributions { get; set; }
    }

    public class DistributionItemVM
    {
        public string Name { get; set; }
        public double Percent { get; set; }
    }
}