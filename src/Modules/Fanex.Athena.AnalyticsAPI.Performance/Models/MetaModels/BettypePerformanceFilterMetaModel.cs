namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;

    public class BettypePerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> CombineIds { get; set; }
    }
}