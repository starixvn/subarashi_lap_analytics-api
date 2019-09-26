using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterMetaModel : GeneralFilterMetaModel
    {
        public SelectMetaModel Measurement { get; set; }
    }
}