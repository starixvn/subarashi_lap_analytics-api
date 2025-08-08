namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;
    using Common.Models.MetaModels;
    using Core.Models.MetaModels;

    public class PerformanceFilterMetaModel : GeneralFilterMetaModel, ISiteNameMetaModel
    {
        public SelectMetaModel Measurement { get; set; }

        public IEnumerable<DistributionMetaModel> TopDistributions { get; set; }
    }
}