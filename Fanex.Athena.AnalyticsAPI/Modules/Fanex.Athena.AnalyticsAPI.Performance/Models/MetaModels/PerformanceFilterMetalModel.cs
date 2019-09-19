using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterMetalModel : GeneralFilterMetalModel
    {
        public SelectMetalModel Measurement { get; set; }
    }
}