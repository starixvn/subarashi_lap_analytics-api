using System;
using System.Data;
using Fanex.Athena.AnalyticsAPI.Common.Attributes;

namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    public static class DataReaderExtensions
    {
        public static bool HasColumn(this IDataRecord reader, string columnName)
        {
            var fieldCount = reader.FieldCount;

            for (var i = 0; i < fieldCount; i++)
            {
                var name = reader.GetName(i);
                if (name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static TResult ReadObject<TResult>(this IDataReader reader) where TResult : new()
        {
            if (reader == null)
            {
                return default;
            }

            var objDTO = Activator.CreateInstance<TResult>();
            var props = objDTO.GetType().GetProperties();
            foreach (var prop in props)
            {
                var cubeMappingAttr = Attribute.GetCustomAttribute(prop, typeof(SsasColumnAttribute)) as SsasColumnAttribute;
                var isValid = cubeMappingAttr != null && !string.IsNullOrWhiteSpace(cubeMappingAttr.MapColumn);
                isValid = isValid && reader.HasColumn(cubeMappingAttr.MapColumn) && reader[cubeMappingAttr.MapColumn] != DBNull.Value;
                if (isValid)
                {
                    prop.SetValue(objDTO, Convert.ChangeType(reader[cubeMappingAttr.MapColumn], prop.PropertyType));
                }
            }

            return objDTO;
        }
    }
}