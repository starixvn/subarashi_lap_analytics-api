namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;
    using Common.Attributes;
    using Common.Models.MetaModels;

    public class DistributionMetaModel
    {
        public string DisplayId { get; set; }

        [Translation(nameof(DisplayId))]
        public string DisplayName { get; set; }

        public IEnumerable<IdentityMetaModel> PreviousPeriodDistributions { get; set; }

        public IEnumerable<IdentityMetaModel> CurrentPeriodDistributions { get; set; }
    }
}