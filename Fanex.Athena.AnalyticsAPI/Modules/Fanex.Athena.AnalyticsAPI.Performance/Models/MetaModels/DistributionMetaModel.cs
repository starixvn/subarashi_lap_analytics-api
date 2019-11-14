using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class DistributionMetaModel
    {
        public string DisplayId { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<IdentityMetaModel> PreviousPeriodDistributions { get; set; }
        public IEnumerable<IdentityMetaModel> CurrentPeriodDistributions { get; set; }
    }
}