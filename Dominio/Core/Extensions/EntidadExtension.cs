using System.Runtime.Serialization;

namespace Dominio.Core.Extensions
{
    public static class EntidadExtension
    {
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
