using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Utils
{
    public interface ISsasCriteria
    {
        [SsasParam(NoneParam = true)]
        bool NeedParams { get; set; }

        IDictionary<string, object> GetParams();

        [SsasParam(NoneParam = true)]
        string Query { get; set; }
    }
}