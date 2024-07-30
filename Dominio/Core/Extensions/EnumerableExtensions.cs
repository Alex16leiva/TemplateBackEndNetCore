namespace Dominio.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Items<T>(this IEnumerable<T> list)
        {
            var isNull = IsNull(list);

            return isNull ? new HashSet<T>() : list;
        }
        public static bool HasItems<T>(this IEnumerable<T> list)
        {
            var isNull = IsNull(list);

            if (isNull) return false;

            return list.Any();
        }
        public static bool IsNull(this object item)
        {
            return item == null;
        }
        public static bool IsNotNull(this object item)
        {
            return item != null;
        }

        public static IEnumerable<T> CopyToList<T>(this IEnumerable<T> list)
        {
            return new List<T>(list);
        }
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return !HasItems(list);
        }
        public static List<T> DistinctList<T>(this IEnumerable<T> list)
        {
            return list.Items().Distinct().ToList();
        }

        public static List<T> ExcludeByPropertyValue<T>(this IEnumerable<T> list, string nameProperty, string value)
        {
            var filteredCollection = new List<T>();
            foreach (var item in list)
            {

                var propertyInfo =
                    item.GetType()
                        .GetProperty(nameProperty);
                if (propertyInfo == null)
                    return list.ToList();

                var propertyValue = propertyInfo.GetValue(item, null);
                if (propertyValue.ToString() != value)
                {
                    filteredCollection.Add(item);
                }
            }

            return filteredCollection;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(s => s.First());
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> funcAsync)
        {
            foreach (T item in enumerable)
            {
                await funcAsync(item);
            }
        }

        public static List<string> GetStringListOf<T>(this IEnumerable<T> collection, string propertyName)
        {
            var result = collection.Select(s => s.GetType().GetProperty(propertyName).GetValue(s, null).ToString());
            return result.Distinct().ToList();

        }

        public static string GetStringFromList<T>(this IEnumerable<T> collection, string separator)
        {
            if (collection.HasItems())
            {
                return string.Join(separator, collection).Trim();
            }
            return string.Empty;
        }
    }
}
