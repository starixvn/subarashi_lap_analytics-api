using System.Collections.Generic;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;
using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class DistributionMetaModel
    {
        public string DisplayId { get; set; }

        [Translation(nameof(DisplayId))]
        public string DisplayName { get; set; }

        public IEnumerable<IdentityMetaModel> PreviousPeriodDistributions { get; set; }

        public IEnumerable<IdentityMetaModel> CurrentPeriodDistributions { get; set; }
    }
}