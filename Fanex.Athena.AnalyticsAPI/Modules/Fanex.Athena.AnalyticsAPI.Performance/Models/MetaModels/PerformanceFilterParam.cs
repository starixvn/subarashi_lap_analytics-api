using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterParam : GeneralFilterParam
    {
        [SsasParam(NoneParam = true)]
        public SelectParam Measurement { get; set; }
    }
}