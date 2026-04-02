namespace Dominio.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Devuelve la colección original si no es nula; 
        /// en caso contrario, devuelve una colección vacía.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección que se desea evaluar.</param>
        /// <returns>
        /// La colección original si no es nula; de lo contrario, una nueva colección vacía.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// IEnumerable<string> nombres = null;
        ///
        /// // Al usar Items(), evitamos excepciones por referencia nula.
        /// foreach (var nombre in nombres.Items())
        /// {
        ///     Console.WriteLine(nombre);
        /// }
        ///
        /// // Salida: (no imprime nada, pero tampoco lanza excepción)
        /// </code>
        /// </example>
        public static IEnumerable<T> Items<T>(this IEnumerable<T> list)
        {
            var isNull = IsNull(list);

            return isNull ? new HashSet<T>() : list;
        }

        /// <summary>
        /// Determina si una colección contiene elementos.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la colección no es nula y contiene al menos un elemento; 
        /// en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// IEnumerable<int> numeros = new List<int> { 1, 2, 3 };
        /// IEnumerable<int> vacia = new List<int>();
        /// IEnumerable<int> nula = null;
        ///
        /// bool tieneNumeros = numeros.HasItems(); // True
        /// bool tieneVacia = vacia.HasItems();     // False
        /// bool tieneNula = nula.HasItems();       // False
        ///
        /// Console.WriteLine($"Numeros: {tieneNumeros}");
        /// Console.WriteLine($"Vacia: {tieneVacia}");
        /// Console.WriteLine($"Nula: {tieneNula}");
        /// </code>
        /// </example>
        public static bool HasItems<T>(this IEnumerable<T> list)
        {
            var isNull = IsNull(list);

            if (isNull) return false;

            return list.Any();
        }

        /// <summary>
        /// Determina si un objeto es nulo.
        /// </summary>
        /// <param name="item">El objeto que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="item"/> es nulo; en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// object objeto1 = null;
        /// object objeto2 = "Hola mundo";
        ///
        /// bool esNulo1 = objeto1.IsNull(); // True
        /// bool esNulo2 = objeto2.IsNull(); // False
        ///
        /// Console.WriteLine($"Objeto1 es nulo: {esNulo1}");
        /// Console.WriteLine($"Objeto2 es nulo: {esNulo2}");
        /// </code>
        /// </example>
        public static bool IsNull(this object item)
        {
            return item == null;
        }

        /// <summary>
        /// Determina si un objeto no es nulo.
        /// </summary>
        /// <param name="item">El objeto que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="item"/> no es nulo; en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// object objeto1 = null;
        /// object objeto2 = "Hola mundo";
        ///
        /// bool noEsNulo1 = objeto1.IsNotNull(); // False
        /// bool noEsNulo2 = objeto2.IsNotNull(); // True
        ///
        /// Console.WriteLine($"Objeto1 no es nulo: {noEsNulo1}");
        /// Console.WriteLine($"Objeto2 no es nulo: {noEsNulo2}");
        /// </code>
        /// </example>
        public static bool IsNotNull(this object item)
        {
            return item != null;
        }

        /// <summary>
        /// Crea una nueva lista a partir de una colección enumerable.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección enumerable que se desea copiar.</param>
        /// <returns>
        /// Una nueva instancia de <see cref="List{T}"/> que contiene los mismos elementos
        /// que la colección original.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// IEnumerable<string> nombres = new[] { "Ana", "Luis", "Carlos" };
        ///
        /// List<string> listaCopiada = nombres.CopyToList();
        ///
        /// foreach (var nombre in listaCopiada)
        /// {
        ///     Console.WriteLine(nombre);
        /// }
        /// // Salida:
        /// // Ana
        /// // Luis
        /// // Carlos
        /// </code>
        /// </example>
        public static IEnumerable<T> CopyToList<T>(this IEnumerable<T> list)
        {
            return new List<T>(list);
        }

        /// <summary>
        /// Determina si una colección está vacía o es nula.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la colección es nula o no contiene elementos; 
        /// en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// IEnumerable<int> numeros = new List<int>();
        /// IEnumerable<int> nula = null;
        /// IEnumerable<int> conDatos = new List<int> { 1, 2, 3 };
        ///
        /// Console.WriteLine(numeros.IsEmpty());   // True
        /// Console.WriteLine(nula.IsEmpty());      // True
        /// Console.WriteLine(conDatos.IsEmpty());  // False
        /// </code>
        /// </example>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return !HasItems(list);
        }

        /// <summary>
        /// Devuelve una lista con los elementos distintos de la colección especificada.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección de la cual se obtendrán los elementos únicos.</param>
        /// <returns>
        /// Una nueva lista de <typeparamref name="T"/> que contiene solo los elementos distintos.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// IEnumerable<int> numeros = new List<int> { 1, 2, 2, 3, 4, 4, 5 };
        ///
        /// List<int> distintos = numeros.DistinctList();
        ///
        /// foreach (var n in distintos)
        /// {
        ///     Console.WriteLine(n);
        /// }
        /// // Salida:
        /// // 1
        /// // 2
        /// // 3
        /// // 4
        /// // 5
        /// </code>
        /// </example>
        public static List<T> DistinctList<T>(this IEnumerable<T> list)
        {
            return list.Items().Distinct().ToList();
        }

        /// <summary>
        /// Excluye de la colección los elementos cuya propiedad especificada
        /// coincide con el valor proporcionado.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="list">La colección de la cual se filtrarán los elementos.</param>
        /// <param name="nameProperty">El nombre de la propiedad a evaluar.</param>
        /// <param name="value">El valor de la propiedad que se desea excluir.</param>
        /// <returns>
        /// Una nueva lista de <typeparamref name="T"/> que contiene los elementos
        /// cuya propiedad <paramref name="nameProperty"/> no coincide con <paramref name="value"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// public class Persona
        /// {
        ///     public string Nombre { get; set; }
        ///     public string Ciudad { get; set; }
        /// }
        ///
        /// var personas = new List<Persona>
        /// {
        ///     new Persona { Nombre = "Ana", Ciudad = "Madrid" },
        ///     new Persona { Nombre = "Luis", Ciudad = "Barcelona" },
        ///     new Persona { Nombre = "Carlos", Ciudad = "Madrid" }
        /// };
        ///
        /// // Excluir las personas cuya Ciudad sea "Madrid"
        /// var filtradas = personas.ExcludeByPropertyValue("Ciudad", "Madrid");
        ///
        /// foreach (var p in filtradas)
        /// {
        ///     Console.WriteLine(p.Nombre);
        /// }
        /// // Salida:
        /// // Luis
        /// </code>
        /// </example>
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

        /// <summary>
        /// Devuelve una colección con elementos distintos de acuerdo a una clave especificada.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <typeparam name="TKey">El tipo de la clave usada para determinar la unicidad.</typeparam>
        /// <param name="enumerable">La colección de la cual se obtendrán los elementos únicos.</param>
        /// <param name="keySelector">
        /// Función que selecciona la clave de cada elemento para evaluar su unicidad.
        /// </param>
        /// <returns>
        /// Una colección de <typeparamref name="T"/> que contiene solo el primer elemento de cada grupo
        /// definido por la clave.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var personas = new List<Persona>
        /// {
        ///     new Persona { Nombre = "Ana", Ciudad = "Madrid" },
        ///     new Persona { Nombre = "Luis", Ciudad = "Barcelona" },
        ///     new Persona { Nombre = "Carlos", Ciudad = "Madrid" }
        /// };
        ///
        /// // Obtener personas distintas por Ciudad
        /// var distintasPorCiudad = personas.DistinctBy(p => p.Ciudad);
        ///
        /// foreach (var p in distintasPorCiudad)
        /// {
        ///     Console.WriteLine($"{p.Nombre} - {p.Ciudad}");
        /// }
        /// // Salida posible:
        /// // Ana - Madrid
        /// // Luis - Barcelona
        /// </code>
        /// </example>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(s => s.First());
        }

        /// <summary>
        /// Ejecuta de manera asíncrona una función sobre cada elemento de la colección.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="enumerable">La colección cuyos elementos se recorrerán.</param>
        /// <param name="funcAsync">
        /// Función asíncrona que se aplicará a cada elemento de la colección.
        /// </param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona de recorrer y ejecutar la función sobre todos los elementos.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// var numeros = new List<int> { 1, 2, 3 };
        ///
        /// await numeros.ForEachAsync(async n =>
        /// {
        ///     await Task.Delay(500); // Simula trabajo asíncrono
        ///     Console.WriteLine($"Procesado: {n}");
        /// });
        ///
        /// // Salida (con retraso de 500ms entre cada elemento):
        /// // Procesado: 1
        /// // Procesado: 2
        /// // Procesado: 3
        /// </code>
        /// </example>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> funcAsync)
        {
            foreach (T item in enumerable)
            {
                await funcAsync(item);
            }
        }

        /// <summary>
        /// Obtiene una lista de valores únicos en formato <see cref="string"/> 
        /// a partir de una propiedad especificada de los elementos de la colección.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="collection">La colección de la cual se extraerán los valores.</param>
        /// <param name="propertyName">El nombre de la propiedad cuyos valores se desean obtener.</param>
        /// <returns>
        /// Una lista de cadenas que representan los valores distintos de la propiedad indicada.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// public class Persona
        /// {
        ///     public string Nombre { get; set; }
        ///     public string Ciudad { get; set; }
        /// }
        ///
        /// var personas = new List<Persona>
        /// {
        ///     new Persona { Nombre = "Ana", Ciudad = "Madrid" },
        ///     new Persona { Nombre = "Luis", Ciudad = "Barcelona" },
        ///     new Persona { Nombre = "Carlos", Ciudad = "Madrid" }
        /// };
        ///
        /// // Obtener lista de ciudades distintas
        /// var ciudades = personas.GetStringListOf("Ciudad");
        ///
        /// foreach (var ciudad in ciudades)
        /// {
        ///     Console.WriteLine(ciudad);
        /// }
        /// // Salida:
        /// // Madrid
        /// // Barcelona
        /// </code>
        /// </example>
        public static List<string> GetStringListOf<T>(this IEnumerable<T> collection, string propertyName)
        {
            var result = collection.Select(s => s.GetType().GetProperty(propertyName).GetValue(s, null).ToString());
            return result.Distinct().ToList();
        }

        /// <summary>
        /// Concatena los elementos de una colección en una sola cadena,
        /// separados por el delimitador especificado.
        /// </summary>
        /// <typeparam name="T">El tipo de elementos contenidos en la colección.</typeparam>
        /// <param name="collection">La colección cuyos elementos se unirán en una cadena.</param>
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
        /// string resultado = nombres.GetStringFromList(", ");
        /// Console.WriteLine(resultado);
        ///
        /// // Salida:
        /// // Ana, Luis, Carlos
        /// </code>
        /// </example>
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
