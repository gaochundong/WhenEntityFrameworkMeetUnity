using System;
using System.Globalization;
using System.Reflection;

namespace WhenEntityFrameworkMeetUnity.BulkExtensions
{
  internal static class TypeExtensions
  {
    public static object GetPrivateFieldValue(this object obj, string propName)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");

      Type type = obj.GetType();

      FieldInfo fieldInfo = null;
      PropertyInfo propertyInfo = null;

      while (fieldInfo == null && propertyInfo == null && type != null)
      {
        fieldInfo = type.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null)
        {
          propertyInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        type = type.BaseType;
      }

      if (fieldInfo == null && propertyInfo == null)
        throw new ArgumentOutOfRangeException("propName",
          string.Format(CultureInfo.InvariantCulture, "Field {0} was not found in Type {1}",
            propName, obj.GetType().FullName));

      if (fieldInfo != null)
        return fieldInfo.GetValue(obj);

      return propertyInfo.GetValue(obj, null);
    }

    public static object GetPropertyValue(this object obj, string propertyName, char separator = '.')
    {
      var segments = propertyName.Split(separator);

      object value = null;
      for (int i = 0; i < segments.Length; ++i)
      {
        object tmp = value ?? obj;
        value = tmp.GetType().GetProperty(segments[i]).GetValue(tmp, null);
      }

      return value;
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, char separator)
    {
      var segments = propertyName.Split(separator);

      PropertyInfo propertyInfo = null;
      for (int i = 0; i < segments.Length; ++i)
      {
        propertyInfo = type.GetProperty(segments[i]);
        type = propertyInfo.PropertyType;
      }

      return propertyInfo;
    }

    public static bool IsNullable(this Type type, out Type argumentType)
    {
      argumentType = null;

      var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
      if (isNullable)
      {
        argumentType = type.GetGenericArguments()[0];
      }

      return isNullable;
    }
  }
}
