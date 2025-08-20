namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    using System.Collections.Generic;

    public class PlatformPerformanceFilterMetaModel : PerformanceFilterMetaModel
    {
        public IEnumerable<string> Platforms { get; set; }
    }
}