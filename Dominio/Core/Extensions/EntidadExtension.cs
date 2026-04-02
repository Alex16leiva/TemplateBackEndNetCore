using System.Runtime.Serialization;

namespace Dominio.Core.Extensions
{
    public static class EntidadExtension
    {
        /// <summary>
        /// Crea una copia profunda de un objeto utilizando <see cref="DataContractSerializer"/>.
        /// </summary>
        /// <typeparam name="T">El tipo del objeto a copiar. Debe ser serializable mediante DataContract.</typeparam>
        /// <param name="theSource">El objeto fuente que se desea clonar.</param>
        /// <returns>
        /// Una nueva instancia de <typeparamref name="T"/> que representa una copia profunda del objeto original.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// [DataContract]
        /// public class Persona
        /// {
        ///     [DataMember]
        ///     public string Nombre { get; set; }
        ///
        ///     [DataMember]
        ///     public int Edad { get; set; }
        /// }
        ///
        /// Persona original = new Persona { Nombre = "Ana", Edad = 30 };
        /// Persona copia = original.DeepCopy();
        ///
        /// Console.WriteLine($"Original: {original.Nombre}, {original.Edad}");
        /// Console.WriteLine($"Copia: {copia.Nombre}, {copia.Edad}");
        /// // Salida: 
        /// // Original: Ana, 30
        /// // Copia: Ana, 30
        /// </code>
        /// </example>
        public static T DeepCopy<T>(this T theSource)
        {
            T theCopy;

            var theDataContactSerializer = new DataContractSerializer(typeof(T));

            using (var memStream = new MemoryStream())
            {
                theDataContactSerializer.WriteObject(memStream, theSource);
                memStream.Position = 0;
                theCopy = (T)theDataContactSerializer.ReadObject(memStream);
            }

            return theCopy;
        }
    }
}
