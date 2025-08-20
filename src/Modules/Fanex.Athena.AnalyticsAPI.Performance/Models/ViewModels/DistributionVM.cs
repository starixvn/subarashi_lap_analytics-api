namespace Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels
{
    using System.Collections.Generic;
    using Common.Attributes;

    public class DistributionVM
    {
        public DistributionVM(int distributionSize = 6)
        {
            Distributions = new List<DistributionItemVM>(distributionSize);
        }

        public string Id { get; set; }

        [Translation(nameof(Id))]
        public string Name { get; set; }

        public double Total { get; set; }

        public IList<DistributionItemVM> Distributions { get; set; }
    }

    public class DistributionItemVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Percent { get; set; }
    }
}