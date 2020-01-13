using Fanex.Athena.LGAPI.SSAS.Bridge.Attributes;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Extensions
{
    public static class MappingExtensions
    {
        public static TResult ToObject<TResult>(this IDictionary<string, object> source)
            where TResult : new()
        {
            if (source == null)
            {
                return default(TResult);
            }

            var objDTO = Activator.CreateInstance<TResult>();
            var props = objDTO.GetType().GetProperties();
            foreach (var prop in props)
            {
                var cubeMappingAttr = Attribute.GetCustomAttribute(prop, typeof(SsasColumnAttribute)) as SsasColumnAttribute;
                var isValid = cubeMappingAttr != null && !string.IsNullOrWhiteSpace(cubeMappingAttr.MapColumn);
                isValid = isValid && source.ContainsKey(cubeMappingAttr.MapColumn) && source[cubeMappingAttr.MapColumn] != null;
                if (isValid)
                {
                    prop.SetValue(objDTO, Convert.ChangeType(source[cubeMappingAttr.MapColumn], prop.PropertyType));
                }
            }

            return objDTO;
        }
    }
}