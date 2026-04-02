namespace Dominio.Core.Extensions
{
    public static class DomainListExtensions
    {
        /// <summary>
        /// Devuelve los elementos de <paramref name="newList"/> que no se encuentran en <paramref name="comparerList"/>,
        /// comparando cada objeto por su clave de igualdad.
        /// </summary>
        /// <param name="newList">La lista principal de elementos a evaluar.</param>
        /// <param name="comparerList">La lista de referencia cuyos elementos se usarán para comparar.</param>
        /// <returns>
        /// Una lista de objetos <see cref="IEqualityKey"/> que están en <paramref name="newList"/> pero no en <paramref name="comparerList"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// // Supongamos que IEqualityKey implementa GetEqualityKey() devolviendo un string único.
        /// List<IEqualityKey> listaNueva = new List<IEqualityKey>
        /// {
        ///     new Persona("Ana"),
        ///     new Persona("Luis"),
        ///     new Persona("Carlos")
        /// };
        ///
        /// List<IEqualityKey> listaComparadora = new List<IEqualityKey>
        /// {
        ///     new Persona("Luis"),
        ///     new Persona("Maria")
        /// };
        ///
        /// List<IEqualityKey> resultado = listaNueva.NotIn(listaComparadora);
        ///
        /// // resultado contendrá: Ana y Carlos
        /// foreach (var item in resultado)
        /// {
        ///     Console.WriteLine(item.GetEqualityKey());
        /// }
        /// </code>
        /// </example>
        public static List<IEqualityKey> NotIn(this IEnumerable<IEqualityKey> newList, IEnumerable<IEqualityKey> comparerList)
        {
            var comparerKeys = comparerList.Items().Select(c => c.GetEqualityKey()).ToList();

            return newList.Items().Where(c => !comparerKeys.Contains(c.GetEqualityKey())).ToList();
        }
    }

    public interface IEqualityKey
    {
        string GetEqualityKey();
    }
}
