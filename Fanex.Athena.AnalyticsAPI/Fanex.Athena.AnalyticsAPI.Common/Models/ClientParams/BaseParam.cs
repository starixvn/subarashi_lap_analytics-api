using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.ClientParams
{
    public abstract class BaseParam : IToParamValue
    {
        public IDictionary<string, object> ToParamValue()
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var property in GetType().GetProperties())
            {
                paramsDic.Add(property.Name, ParsePropertyValue(property.GetValue(this)));
            }

            return paramsDic;
        }

        protected virtual object ParsePropertyValue(object value)
        {
            //to do
            return value;
        }
    }
}