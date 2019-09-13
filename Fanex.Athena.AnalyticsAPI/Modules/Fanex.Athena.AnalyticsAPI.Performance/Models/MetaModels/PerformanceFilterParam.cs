using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterParam : GeneralFilterParam
    {
        public SelectParam Measurement { get; set; }
    }
}