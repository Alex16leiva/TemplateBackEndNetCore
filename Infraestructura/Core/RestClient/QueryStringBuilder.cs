using System.Collections;
using System.Reflection;
using System.Web;

namespace Infraestructura.Core.RestClient
{
    public static class QueryStringBuilder
    {
        /// <summary>
        /// Covert an object into a query string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object to be converted into a query string.</param>
        /// <returns>The converted query string.</returns>
        public static string GetQueryString<T>(T obj)
        {
            IEnumerable<PropertyInfo> propertyInfos = from p in obj.GetType().GetProperties()
                                                      where p.GetValue(obj, null) != null
                                                      select p;

            List<string> propertiesStringBuilder = new List<string>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = propertyInfo.PropertyType;

                if (IsSimpleType(propertyType))
                {
                    propertiesStringBuilder.Add(GetSimpleTypeValue(obj, propertyInfo));
                }
                else
                {
                    string complexTypeValue = GetComplexTypeValue(obj, propertyInfo);
                    if (!string.IsNullOrWhiteSpace(complexTypeValue))
                    {
                        propertiesStringBuilder.Add(complexTypeValue);
                    }
                }
            }

            return string.Join("&", propertiesStringBuilder.ToArray());
        }

        private static string GetSimpleTypeValue<T>(T obj, PropertyInfo propertyInfo)
        {
            // For primitive types we just need to get the property value.
            return $"{propertyInfo.Name}={HttpUtility.UrlEncode(propertyInfo.GetValue(obj, null).ToString())}";
        }

        private static string GetComplexTypeValue<T>(T obj, PropertyInfo propertyInfo)
        {
            // For complex types first we need to figure out if the property is a collection or not.
            if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
            {
                List<string> propertiesStringBuilder = new List<string>();
                string collectionPropertyName = propertyInfo.Name;

                Type collectionType = propertyInfo.PropertyType.GetGenericArguments()[0];

                if (IsSimpleType(collectionType))
                {
                    IEnumerable collection = (IEnumerable)propertyInfo.GetValue(obj, null);

                    foreach (var item in collection)
                    {
                        if (item != null)
                        {
                            string collectionItem = $"{collectionPropertyName}={HttpUtility.UrlEncode(item.ToString())}";

                            propertiesStringBuilder.Add(collectionItem);
                        }
                    }
                }

                return string.Join("&", propertiesStringBuilder.ToArray());
            }

            return string.Empty;
        }

        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                              typeof (Enum),
                              typeof (string),
                              typeof (char),
                              typeof (Guid),
                              typeof (bool),
                              typeof (byte),
                              typeof (short),
                              typeof (int),
                              typeof (long),
                              typeof (float),
                              typeof (double),
                              typeof (decimal),
                              typeof (sbyte),
                              typeof (ushort),
                              typeof (uint),
                              typeof (ulong),
                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }
    }
}
