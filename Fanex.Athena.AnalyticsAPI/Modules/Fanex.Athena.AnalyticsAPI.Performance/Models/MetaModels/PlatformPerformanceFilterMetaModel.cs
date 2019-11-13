using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PlatformPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<int> Platforms { get; set; }
    }
}