using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PlatformPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Platforms { get; set; }
    }
}