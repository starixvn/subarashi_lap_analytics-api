using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels
{
    public class SelectParam : IToParamValue
    { 
        [ParamQueryMapping("baseCurrency")]
        public int Id { get; set; }

        [ParamQueryMapping(NoneParam =true)]
        public string Name { get; set; }

        public IDictionary<string, object> ToParamValue()
        {
            return null;
        }
    }
}