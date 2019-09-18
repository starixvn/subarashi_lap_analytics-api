using System;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SsasParamAttribute : Attribute
    {
        public SsasParamAttribute(string paramName, bool noneParam = false)
        {
            ParamName = paramName;
            NoneParam = noneParam;
        }

        public SsasParamAttribute()
        {
        }

        public string ParamName { get; set; }
        public bool NoneParam { get; set; }
    }
}