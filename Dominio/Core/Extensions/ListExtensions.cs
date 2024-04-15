using System.Data;

namespace Dominio.Core.Extensions
{
    public static class ListExtensions
    {
        public static string ToStringList(this IEnumerable<string> list)
        {
            if (list != null && list.Any())
            {
                return string.Join(", ", list);
            }

            return string.Empty;
        }

        public static string ToStringList(this IEnumerable<string> list, string separator)
        {
            if (list != null && list.Any())
            {
                return string.Join(separator, list);
            }

            return string.Empty;
        }

        public static bool HasEqualsAllItems(this IEnumerable<string> list, IEnumerable<string> list2)
        {
            if (!list.HasItems() && !list2.HasItems())
            {
                return true;
            }

            var differenceInSourceList = list.Except(list2);
            var differenceInTargetList = list2.Except(list);

            if (!differenceInSourceList.Any() && !differenceInTargetList.Any())
            {
                return true;
            }
            return false;
        }

        public static bool HasEqualSomeItemsOf(this IEnumerable<string> sourceList, IEnumerable<string> list)
        {
            if (list.All(sourceList.Contains))
            {
                return true;
            }
            return false;
        }

        public static DataTable ToDataTable(this IEnumerable<string> collection, string fieldId)
        {
            var tabla = new DataTable();
            tabla.Columns.Add(fieldId, typeof(string));
            foreach (var estilo in collection)
            {
                var dr = tabla.NewRow();
                dr[fieldId] = estilo;
                tabla.Rows.Add(dr);
            }
            return tabla;
        }

    }
}
