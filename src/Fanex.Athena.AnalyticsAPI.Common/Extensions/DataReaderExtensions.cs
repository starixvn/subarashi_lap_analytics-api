namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    using System;
    using System.Data;
    using Attributes;

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

        private static object GetDefaultValue(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null;
            }

            if (type == typeof(string))
            {
                return string.Empty;
            }

            if (type.IsValueType)
            {
                var typeCode = Type.GetTypeCode(type);
                if (typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal)
                {
                    return 0;
                }

                return typeCode switch
                {
                    TypeCode.Boolean => false,
                    TypeCode.DateTime => DateTimeOffset.MinValue.DateTime,
                    _ => Activator.CreateInstance(type),
                };
            }

            return null;
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
                var isValid = cubeMappingAttr != null && !string.IsNullOrWhiteSpace(cubeMappingAttr.MapColumn) && reader.HasColumn(cubeMappingAttr.MapColumn);

                if (!isValid)
                {
                    continue;
                }

                var value = reader[cubeMappingAttr.MapColumn];
                var isNotNull = value != DBNull.Value && value is not DBNull &&
                                !(value is string strValue && strValue.Equals("NULL", StringComparison.OrdinalIgnoreCase));

                prop.SetValue(objDTO, isNotNull ? Convert.ChangeType(value, prop.PropertyType) : GetDefaultValue(prop.PropertyType));
            }

            return objDTO;
        }
    }
}