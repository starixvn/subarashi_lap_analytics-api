using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class BettypePerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> CombineIds { get; set; }
    }
}