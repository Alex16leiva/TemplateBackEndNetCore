using System.Data;

namespace Dominio.Core.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Convierte una colección de cadenas en una sola cadena,
        /// separando cada elemento con una coma y un espacio.
        /// </summary>
        /// <param name="list">La colección de cadenas que se desea unir.</param>
        /// <returns>
        /// Una cadena que contiene los elementos de la colección separados por ", ".
        /// Si la colección es nula o está vacía, devuelve una cadena vacía.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var nombres = new List<string> { "Ana", "Luis", "Carlos" };
        ///
        /// string resultado = nombres.ToStringList();
        /// Console.WriteLine(resultado);
        ///
        /// // Salida:
        /// // Ana, Luis, Carlos
        /// </code>
        /// </example>
        public static string ToStringList(this IEnumerable<string> list)
        {
            if (list.HasItems())
            {
                return string.Join(", ", list);
            }

            return string.Empty;
        }

        /// <summary>
        /// Convierte una colección de cadenas en una sola cadena,
        /// separando cada elemento con el delimitador especificado.
        /// </summary>
        /// <param name="list">La colección de cadenas que se desea unir.</param>
        /// <param name="separator">El separador que se usará entre los elementos.</param>
        /// <returns>
        /// Una cadena que contiene los elementos de la colección separados por <paramref name="separator"/>.
        /// Si la colección es nula o está vacía, devuelve una cadena vacía.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var nombres = new List<string> { "Ana", "Luis", "Carlos" };
        ///
        /// string resultado1 = nombres.ToStringList(", ");
        /// Console.WriteLine(resultado1);
        /// // Salida: Ana, Luis, Carlos
        ///
        /// string resultado2 = nombres.ToStringList(" | ");
        /// Console.WriteLine(resultado2);
        /// // Salida: Ana | Luis | Carlos
        /// </code>
        /// </example>
        public static string ToStringList(this IEnumerable<string> list, string separator)
        {
            if (list.HasItems())
            {
                return string.Join(separator, list);
            }

            return string.Empty;
        }

        /// <summary>
        /// Determina si dos colecciones de cadenas contienen exactamente los mismos elementos,
        /// independientemente del orden.
        /// </summary>
        /// <param name="list">La primera colección de cadenas.</param>
        /// <param name="list2">La segunda colección de cadenas.</param>
        /// <returns>
        /// <c>true</c> si ambas colecciones son nulas o vacías, o si contienen los mismos elementos;
        /// en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var lista1 = new List<string> { "Ana", "Luis", "Carlos" };
        /// var lista2 = new List<string> { "Carlos", "Luis", "Ana" };
        /// var lista3 = new List<string> { "Ana", "Luis" };
        ///
        /// bool iguales1 = lista1.HasEqualsAllItems(lista2); // True
        /// bool iguales2 = lista1.HasEqualsAllItems(lista3); // False
        ///
        /// Console.WriteLine($"Lista1 y Lista2 iguales: {iguales1}");
        /// Console.WriteLine($"Lista1 y Lista3 iguales: {iguales2}");
        /// </code>
        /// </example>
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

        /// <summary>
        /// Determina si todos los elementos de una colección están contenidos en otra colección.
        /// </summary>
        /// <param name="sourceList">La colección en la que se buscarán los elementos.</param>
        /// <param name="list">La colección cuyos elementos deben estar presentes en <paramref name="sourceList"/>.</param>
        /// <returns>
        /// <c>true</c> si todos los elementos de <paramref name="list"/> están contenidos en <paramref name="sourceList"/>; 
        /// en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var lista1 = new List<string> { "Ana", "Luis", "Carlos" };
        /// var lista2 = new List<string> { "Luis", "Ana" };
        /// var lista3 = new List<string> { "Pedro", "Ana" };
        ///
        /// bool contiene1 = lista1.HasEqualSomeItemsOf(lista2); // True
        /// bool contiene2 = lista1.HasEqualSomeItemsOf(lista3); // False
        ///
        /// Console.WriteLine($"Lista1 contiene todos los de Lista2: {contiene1}");
        /// Console.WriteLine($"Lista1 contiene todos los de Lista3: {contiene2}");
        /// </code>
        /// </example>
        public static bool HasEqualSomeItemsOf(this IEnumerable<string> sourceList, IEnumerable<string> list)
        {
            if (list.All(sourceList.Contains))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convierte una colección de cadenas en un <see cref="DataTable"/> con una única columna.
        /// </summary>
        /// <param name="collection">La colección de cadenas que se desea transformar en tabla.</param>
        /// <param name="fieldId">El nombre de la columna que se creará en el <see cref="DataTable"/>.</param>
        /// <returns>
        /// Un <see cref="DataTable"/> con una columna llamada <paramref name="fieldId"/> 
        /// y una fila por cada elemento de la colección.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var estilos = new List<string> { "Clásico", "Moderno", "Minimalista" };
        ///
        /// DataTable tabla = estilos.ToDataTable("Estilo");
        ///
        /// foreach (DataRow fila in tabla.Rows)
        /// {
        ///     Console.WriteLine(fila["Estilo"]);
        /// }
        ///
        /// // Salida:
        /// // Clásico
        /// // Modernista
        /// // Minimalista
        /// </code>
        /// </example>
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
