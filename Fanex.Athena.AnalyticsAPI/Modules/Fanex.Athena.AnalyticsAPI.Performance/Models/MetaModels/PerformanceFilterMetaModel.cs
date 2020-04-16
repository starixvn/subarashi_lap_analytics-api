using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Core.Models.MetaModels;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterMetaModel : GeneralFilterMetaModel, ISiteNameMetaModel
    {
        public SelectMetaModel Measurement { get; set; }

        public IEnumerable<DistributionMetaModel> TopDistributions { get; set; }
    }
}