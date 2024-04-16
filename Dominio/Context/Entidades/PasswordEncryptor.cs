using Dominio.Core.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Context.Entidades
{
    public static class PasswordEncryptor
    {
        public static string Encrypt(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convierte la cadena de entrada en un arreglo de bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Calcula el hash SHA-256
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convierte el hash en una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToStringValue();
            }
        }
    }
}