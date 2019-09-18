using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Utils
{
    public class BaseSsasCriteria : ISsasCriteria
    {
        protected BaseSsasCriteria()
        {
        }

        [SsasParam(NoneParam = true)]
        public virtual string Query { get; set; }

        [SsasParam(NoneParam = true)]
        public virtual bool NeedParams { get; set; } = true;

        public IDictionary<string, object> GetParams()
        {
            var paramsDic = new Dictionary<string, object>();
            if (NeedParams)
            {
                var props = this.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var paramQueryAttribute = Attribute.GetCustomAttribute(prop, typeof(SsasParamAttribute)) as SsasParamAttribute;

                    if (paramQueryAttribute == null)
                    {
                        paramsDic.Add(prop.Name, prop.GetValue(this));
                    }
                    else if (!paramQueryAttribute.NoneParam && !string.IsNullOrWhiteSpace(paramQueryAttribute.ParamName))
                    {
                        paramsDic.Add(paramQueryAttribute.ParamName, prop.GetValue(this));
                    }
                    else
                    {
                        //
                    }
                }
            }

            return paramsDic;
        }
    }
}