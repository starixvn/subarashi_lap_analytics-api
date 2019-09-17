using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels
{
    public class PerformanceFilterParam : GeneralFilterParam
    {
        [ParamQueryMapping(NoneParam = true)]
        public SelectParam Measurement { get; set; }
    }
}