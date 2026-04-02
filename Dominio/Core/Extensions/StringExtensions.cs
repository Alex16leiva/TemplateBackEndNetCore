using System.Text.RegularExpressions;

namespace Dominio.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convierte una cadena en un arreglo de objetos con un único elemento,
        /// asegurando que el valor no sea nulo mediante <c>ValueOrEmpty()</c>.
        /// </summary>
        /// <param name="value">La cadena que se desea convertir.</param>
        /// <returns>
        /// Un arreglo de objetos que contiene un único elemento:
        /// la cadena original o una cadena vacía si es nula.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Hola";
        /// string texto2 = null;
        ///
        /// object[] resultado1 = texto1.ToObject(); // { "Hola" }
        /// object[] resultado2 = texto2.ToObject(); // { "" }
        ///
        /// Console.WriteLine($"Resultado1: {resultado1[0]}");
        /// Console.WriteLine($"Resultado2: {resultado2[0]}");
        /// </code>
        /// </example>
        public static object[] ToObject(this string value)
        {
            return new object[] { value.ValueOrEmpty() };
        }

        /// <summary>
        /// Convierte una cadena en un valor <see cref="decimal"/>.
        /// Si la cadena es nula, vacía o no puede convertirse, devuelve 0.
        /// </summary>
        /// <param name="decimalStringValue">La cadena que representa un número decimal.</param>
        /// <returns>
        /// El valor convertido a <see cref="decimal"/> si la cadena es válida; en caso contrario, 0.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "123.45";
        /// string texto2 = "abc";
        /// string texto3 = null;
        ///
        /// decimal resultado1 = texto1.ToDecimal(); // 123.45
        /// decimal resultado2 = texto2.ToDecimal(); // 0
        /// decimal resultado3 = texto3.ToDecimal(); // 0
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// Console.WriteLine($"Resultado3: {resultado3}");
        /// </code>
        /// </example>
        public static decimal ToDecimal(this string decimalStringValue)
        {
            decimal decimalValue = 0;
            if (decimalStringValue.HasValue())
            {
                Decimal.TryParse(decimalStringValue, out decimalValue);
            }
            return decimalValue;
        }

        /// <summary>
        /// Convierte una cadena en un valor <see cref="int"/>.
        /// Si la cadena es nula, vacía o no puede convertirse, devuelve 0.
        /// </summary>
        /// <param name="decimalStringValue">La cadena que representa un número entero.</param>
        /// <returns>
        /// El valor convertido a <see cref="int"/> si la cadena es válida; en caso contrario, 0.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "123";
        /// string texto2 = "abc";
        /// string texto3 = null;
        ///
        /// int resultado1 = texto1.ToInt(); // 123
        /// int resultado2 = texto2.ToInt(); // 0
        /// int resultado3 = texto3.ToInt(); // 0
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// Console.WriteLine($"Resultado3: {resultado3}");
        /// </code>
        /// </example>
        public static int ToInt(this string decimalStringValue)
        {
            int decimalValue = 0;
            if (decimalStringValue.HasValue())
            {
                Int32.TryParse(decimalStringValue, out decimalValue);
            }
            return decimalValue;
        }

        /// <summary>
        /// Extrae todos los caracteres numéricos de una cadena,
        /// devolviendo una nueva cadena compuesta únicamente por dígitos.
        /// </summary>
        /// <param name="decimalStringValue">La cadena de entrada que puede contener números y otros caracteres.</param>
        /// <returns>
        /// Una cadena que contiene únicamente los dígitos presentes en <paramref name="decimalStringValue"/>.
        /// Si la cadena es nula o vacía, devuelve una cadena vacía.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Tel: +504-9876-1234";
        /// string texto2 = "Precio: $123.45";
        /// string texto3 = null;
        ///
        /// string resultado1 = texto1.GetNumericValues(); // "50498761234"
        /// string resultado2 = texto2.GetNumericValues(); // "12345"
        /// string resultado3 = texto3.GetNumericValues(); // ""
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// Console.WriteLine($"Resultado3: {resultado3}");
        /// </code>
        /// </example>
        public static string GetNumericValues(this string decimalStringValue)
        {
            string output = string.Empty;
            if (decimalStringValue.HasValue())
            {
                output = new string(decimalStringValue.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            }
            return output;
        }

        /// <summary>
        /// Verifica si una cadena tiene un valor válido,
        /// es decir, que no sea nula, vacía ni compuesta únicamente por espacios en blanco.
        /// </summary>
        /// <param name="stringValue">La cadena que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la cadena contiene un valor válido; en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Hola";
        /// string texto2 = "";
        /// string texto3 = "   ";
        /// string texto4 = null;
        ///
        /// bool resultado1 = texto1.HasValue(); // true
        /// bool resultado2 = texto2.HasValue(); // false
        /// bool resultado3 = texto3.HasValue(); // false
        /// bool resultado4 = texto4.HasValue(); // false
        ///
        /// Console.WriteLine($"Texto1: {resultado1}, Texto2: {resultado2}, Texto3: {resultado3}, Texto4: {resultado4}");
        /// </code>
        /// </example>
        public static bool HasValue(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue) && !string.IsNullOrWhiteSpace(stringValue);
        }

        /// <summary>
        /// Devuelve la cadena original si contiene un valor válido,
        /// o una cadena vacía si es nula, vacía o compuesta únicamente por espacios.
        /// </summary>
        /// <param name="stringValue">La cadena que se desea evaluar.</param>
        /// <returns>
        /// La cadena original recortada con <see cref="string.Trim"/> si tiene valor;
        /// en caso contrario, <see cref="string.Empty"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = " Hola ";
        /// string texto2 = "";
        /// string texto3 = "   ";
        /// string texto4 = null;
        ///
        /// string resultado1 = texto1.ValueOrEmpty(); // "Hola"
        /// string resultado2 = texto2.ValueOrEmpty(); // ""
        /// string resultado3 = texto3.ValueOrEmpty(); // ""
        /// string resultado4 = texto4.ValueOrEmpty(); // ""
        ///
        /// Console.WriteLine($"Texto1: '{resultado1}', Texto2: '{resultado2}', Texto3: '{resultado3}', Texto4: '{resultado4}'");
        /// </code>
        /// </example>
        public static string ValueOrEmpty(this string stringValue)
        {
            return HasValue(stringValue) ? stringValue.Trim() : string.Empty;
        }

        /// <summary>
        /// Convierte un objeto en su representación de cadena.
        /// Si el objeto es nulo, devuelve una cadena vacía.
        /// </summary>
        /// <param name="stringValue">El objeto que se desea convertir a cadena.</param>
        /// <returns>
        /// La representación en cadena del objeto, recortada con <see cref="string.Trim"/> si no es nulo;
        /// en caso contrario, <see cref="string.Empty"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// object valor1 = " Hola ";
        /// object valor2 = 123;
        /// object valor3 = null;
        ///
        /// string resultado1 = valor1.ToStringValue(); // "Hola"
        /// string resultado2 = valor2.ToStringValue(); // "123"
        /// string resultado3 = valor3.ToStringValue(); // ""
        ///
        /// Console.WriteLine($"Resultado1: '{resultado1}', Resultado2: '{resultado2}', Resultado3: '{resultado3}'");
        /// </code>
        /// </example>
        public static string ToStringValue(this object stringValue)
        {
            return stringValue != null ? stringValue.ToString().Trim() : string.Empty;
        }

        /// <summary>
        /// Verifica si una cadena está vacía, es nula o contiene únicamente espacios en blanco.
        /// </summary>
        /// <param name="stringValue">La cadena que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la cadena no tiene un valor válido (es nula, vacía o solo espacios);
        /// en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Hola";
        /// string texto2 = "";
        /// string texto3 = "   ";
        /// string texto4 = null;
        ///
        /// bool resultado1 = texto1.IsMissingValue(); // false
        /// bool resultado2 = texto2.IsMissingValue(); // true
        /// bool resultado3 = texto3.IsMissingValue(); // true
        /// bool resultado4 = texto4.IsMissingValue(); // true
        ///
        /// Console.WriteLine($"Texto1: {resultado1}, Texto2: {resultado2}, Texto3: {resultado3}, Texto4: {resultado4}");
        /// </code>
        /// </example>
        public static bool IsMissingValue(this string stringValue)
        {
            return !HasValue(stringValue);
        }

        /// <summary>
        /// Divide una cadena en una lista de subcadenas.
        /// Si se proporcionan separadores, utiliza el primero de ellos.
        /// Si no se proporcionan, divide la cadena por saltos de línea.
        /// </summary>
        /// <param name="value">La cadena que se desea dividir.</param>
        /// <param name="separators">
        /// Opcional: uno o más caracteres separadores. 
        /// Si se especifican, se usa el primero para realizar la división.
        /// </param>
        /// <returns>
        /// Una lista de subcadenas obtenidas a partir de <paramref name="value"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "uno,dos,tres";
        /// string texto2 = "linea1\r\nlinea2\r\nlinea3";
        ///
        /// List<string> lista1 = texto1.SplitIntoList(','); 
        /// // { "uno", "dos", "tres" }
        ///
        /// List<string> lista2 = texto2.SplitIntoList(); 
        /// // { "linea1", "linea2", "linea3" }
        ///
        /// Console.WriteLine(string.Join(" | ", lista1));
        /// Console.WriteLine(string.Join(" | ", lista2));
        /// </code>
        /// </example>
        public static List<string> SplitIntoList(this string value, params char[] separators)
        {
            if (separators.HasItems())
            {
                var firstSeparator = separators.First();
                var messagesKeys = value.Split(firstSeparator);
                return messagesKeys.ToList();
            }
            else
            {
                var observaciones = value.Replace("\r\n", "\n");
                var messagesKeys = observaciones.Split('\n');
                return messagesKeys.ToList();
            }
        }

        /// <summary>
        /// Divide una cadena en una lista de subcadenas utilizando separadores,
        /// y opcionalmente elimina caracteres especiales definidos en <paramref name="toExclude"/>.
        /// </summary>
        /// <param name="value">La cadena que se desea dividir.</param>
        /// <param name="toExclude">
        /// Conjunto de caracteres que se deben eliminar de cada subcadena resultante.
        /// Si está vacío o nulo, no se eliminan caracteres adicionales.
        /// </param>
        /// <param name="separators">
        /// Uno o más caracteres separadores. 
        /// Si se especifican, se usa el primero para realizar la división.
        /// Si no se especifican, se divide por saltos de línea.
        /// </param>
        /// <returns>
        /// Una lista de subcadenas obtenidas a partir de <paramref name="value"/>,
        /// con los caracteres excluidos eliminados si corresponde.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "uno@,dos#,tres$";
        /// var lista1 = texto1.SplitIntoListWithOutSpecialCharacters(new[] { '@', '#', '$' }, ',');
        /// // { "uno", "dos", "tres" }
        ///
        /// string texto2 = "linea1\r\nlinea2\r\nlinea3";
        /// var lista2 = texto2.SplitIntoListWithOutSpecialCharacters(null);
        /// // { "linea1", "linea2", "linea3" }
        ///
        /// Console.WriteLine(string.Join(" | ", lista1));
        /// Console.WriteLine(string.Join(" | ", lista2));
        /// </code>
        /// </example>
        public static List<string> SplitIntoListWithOutSpecialCharacters(this string value, IEnumerable<char> toExclude, params char[] separators)
        {
            if (separators.HasItems())
            {
                var firstSeparator = separators.First();
                var messagesKeys = value.Split(firstSeparator);
                var returnList = messagesKeys.ToList();
                var result = new List<string>();
                if (toExclude.HasItems())
                {
                    foreach (var item in returnList)
                    {
                        var newString = item.Trim();
                        foreach (var forbidenItem in toExclude)
                        {
                            newString.Replace(forbidenItem.ToString(), string.Empty);
                        }
                        result.Add(newString);
                    }
                    return result;
                }
                return returnList;
            }
            else
            {
                var observaciones = value.Replace("\r\n", "\n");
                var messagesKeys = observaciones.Split('\n');
                return messagesKeys.ToList();
            }
        }

        /// <summary>
        /// Convierte una cadena en un valor <see cref="DateTime"/>.
        /// Si la cadena es nula, vacía o no puede convertirse, devuelve <see cref="DateTime.MinValue"/>.
        /// </summary>
        /// <param name="dateStringValue">La cadena que representa una fecha.</param>
        /// <returns>
        /// El valor convertido a <see cref="DateTime"/> si la cadena es válida; 
        /// en caso contrario, <see cref="DateTime.MinValue"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "2026-04-02";
        /// string texto2 = "02/04/2026 16:30";
        /// string texto3 = "fecha inválida";
        /// string texto4 = null;
        ///
        /// DateTime resultado1 = texto1.ToDateTime(); // 2026-04-02 00:00:00
        /// DateTime resultado2 = texto2.ToDateTime(); // 2026-04-02 16:30:00
        /// DateTime resultado3 = texto3.ToDateTime(); // DateTime.MinValue
        /// DateTime resultado4 = texto4.ToDateTime(); // DateTime.MinValue
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// Console.WriteLine($"Resultado3: {resultado3}");
        /// Console.WriteLine($"Resultado4: {resultado4}");
        /// </code>
        /// </example>
        public static DateTime ToDateTime(this string dateStringValue)
        {
            DateTime dateValue = DateTime.MinValue;
            if (dateStringValue.HasValue())
            {
                DateTime.TryParse(dateStringValue, out dateValue);
            }
            return dateValue;
        }

        /// <summary>
        /// Inserta espacios antes de cada letra mayúscula en una cadena,
        /// devolviendo el resultado con un formato más legible.
        /// </summary>
        /// <param name="value">La cadena de entrada que contiene letras mayúsculas.</param>
        /// <returns>
        /// Una nueva cadena con espacios añadidos antes de cada letra mayúscula.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "HolaMundo";
        /// string texto2 = "XMLParserExtension";
        ///
        /// string resultado1 = texto1.AddSpacesBeforeCapitalLetters(); 
        /// // "Hola Mundo"
        ///
        /// string resultado2 = texto2.AddSpacesBeforeCapitalLetters(); 
        /// // "XML Parser Extension"
        ///
        /// Console.WriteLine(resultado1);
        /// Console.WriteLine(resultado2);
        /// </code>
        /// </example>
        public static string AddSpacesBeforeCapitalLetters(this string value)
        {
            return string.Concat(value.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }

        /// <summary>
        /// Verifica si una cadena contiene únicamente caracteres numéricos (dígitos).
        /// </summary>
        /// <param name="str">La cadena que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la cadena contiene solo dígitos.
        /// Si la cadena es nula o vacía, devuelve <c>true</c> por diseño.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "12345";
        /// string texto2 = "12a45";
        /// string texto3 = "";
        /// string texto4 = null;
        ///
        /// bool resultado1 = texto1.IsNumeric(); // true
        /// bool resultado2 = texto2.IsNumeric(); // false
        /// bool resultado3 = texto3.IsNumeric(); // true
        /// bool resultado4 = texto4.IsNumeric(); // true
        ///
        /// Console.WriteLine($"Texto1: {resultado1}, Texto2: {resultado2}, Texto3: {resultado3}, Texto4: {resultado4}");
        /// </code>
        /// </example>
        public static bool IsNumeric(this string str)
        {
            if (str.HasValue())
            {
                return !str.ToArray().Any(a => !char.IsDigit(a));
            }
            return true;
        }

        /// <summary>
        /// Verifica si una cadena representa un número decimal válido.
        /// Se permiten dígitos y los caracteres ',' y '.' como separadores decimales.
        /// </summary>
        /// <param name="str">La cadena que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si la cadena contiene únicamente dígitos y opcionalmente
        /// los caracteres ',' o '.'; en caso contrario, <c>false</c>.
        /// Si la cadena es nula o vacía, devuelve <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "123.45";
        /// string texto2 = "123,45";
        /// string texto3 = "12a45";
        /// string texto4 = "";
        /// string texto5 = null;
        ///
        /// bool resultado1 = texto1.IsDecimal(); // true
        /// bool resultado2 = texto2.IsDecimal(); // true
        /// bool resultado3 = texto3.IsDecimal(); // false
        /// bool resultado4 = texto4.IsDecimal(); // false
        /// bool resultado5 = texto5.IsDecimal(); // false
        ///
        /// Console.WriteLine($"Texto1: {resultado1}, Texto2: {resultado2}, Texto3: {resultado3}, Texto4: {resultado4}, Texto5: {resultado5}");
        /// </code>
        /// </example>
        public static bool IsDecimal(this string str)
        {
            if (str.IsMissingValue())
            {
                return false;
            }

            List<char> charsToexclude = new List<char> { ',', '.' };
            if (str.HasValue())
            {
                var arr = str.ToArray();
                var chars = arr.Where(w => !char.IsDigit(w) && !charsToexclude.Contains(w));

                return !chars.Any();

            }
            return true;
        }

        /// <summary>
        /// Limita la longitud de una cadena a un máximo especificado.
        /// Si la cadena es más corta o igual al máximo, se devuelve completa.
        /// Si es más larga, se devuelve truncada.
        /// </summary>
        /// <param name="value">La cadena que se desea truncar.</param>
        /// <param name="maxLength">La longitud máxima permitida.</param>
        /// <returns>
        /// La cadena original si su longitud es menor o igual a <paramref name="maxLength"/>; 
        /// en caso contrario, una subcadena de longitud máxima.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Hola Mundo";
        /// string texto2 = "Este texto es demasiado largo";
        ///
        /// string resultado1 = texto1.Truncate(20); // "Hola Mundo"
        /// string resultado2 = texto2.Truncate(10); // "Este texto"
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Elimina todos los espacios y caracteres de espacio en blanco de una cadena,
        /// devolviendo el resultado sin espacios.
        /// </summary>
        /// <param name="str">La cadena de entrada que se desea limpiar.</param>
        /// <returns>
        /// Una nueva cadena sin espacios ni caracteres de espacio en blanco.
        /// Si la cadena es nula, devuelve <c>null</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// string texto1 = "Hola Mundo";
        /// string texto2 = " 123 \t 456 ";
        /// string texto3 = null;
        ///
        /// string resultado1 = texto1.RemoveSpaceEmpty(); // "HolaMundo"
        /// string resultado2 = texto2.RemoveSpaceEmpty(); // "123456"
        /// string resultado3 = texto3.RemoveSpaceEmpty(); // null
        ///
        /// Console.WriteLine($"Resultado1: '{resultado1}'");
        /// Console.WriteLine($"Resultado2: '{resultado2}'");
        /// Console.WriteLine($"Resultado3: '{resultado3}'");
        /// </code>
        /// </example>
        public static string RemoveSpaceEmpty(this string str)
        {
            return Regex.Replace(str, @"\s", "")?.Trim();
        }

        /// <summary>
        /// Extensión para cadenas que permite insertar parámetros dinámicos
        /// usando <see cref="string.Format(string, object[])"/>.
        /// </summary>
        /// <param name="str">
        /// Cadena base que contiene placeholders (ej: "Hola {0}, tienes {1} mensajes").
        /// </param>
        /// <param name="parameters">
        /// Arreglo de objetos que reemplazarán los placeholders en la cadena.
        /// </param>
        /// <returns>
        /// Una nueva cadena con los parámetros reemplazados.
        /// </returns>
        /// <example>
        /// Ejemplo 1:
        /// string saludo = "Hola {0}".AddStringParameters(new object[] { "Alexander" });
        /// // Resultado: "Hola Alexander"
        ///
        /// Ejemplo 2:
        /// string info = "El producto {0} cuesta {1:C}".AddStringParameters(new object[] { "Laptop", 1200 });
        /// // Resultado: "El producto Laptop cuesta $1,200.00"
        ///
        /// Ejemplo 3:
        /// string fecha = "Hoy es {0:dddd}, {0:dd/MM/yyyy}".AddStringParameters(new object[] { DateTime.Now });
        /// // Resultado: "Hoy es jueves, 02/04/2026"
        /// </example>
        public static string AddStringParameters(this string str, object[] parameters)
        {
            return string.Format(str, parameters);
        }

        /// <summary>
        /// Extensión para ordenar alfabéticamente los elementos de una cadena
        /// separados por un delimitador específico.
        /// </summary>
        /// <param name="strUnordered">
        /// Cadena original que contiene elementos separados por el delimitador.
        /// Ejemplo: "perro,gato,ave".
        /// </param>
        /// <param name="parameters">
        /// Carácter delimitador que separa los elementos.
        /// Ejemplo: ',' o ';'.
        /// </param>
        /// <returns>
        /// Una nueva cadena con los elementos ordenados alfabéticamente.
        /// Si la cadena está vacía o no contiene el delimitador, se devuelve la original.
        /// </returns>
        /// <example>
        /// Ejemplo 1:
        /// string animales = "perro,gato,ave".OrderStringAscBySeparator(',');
        /// // Resultado: "ave,gato,perro"
        ///
        /// Ejemplo 2:
        /// string frutas = "Mango;manzana;Banana".OrderStringAscBySeparator(';');
        /// // Resultado: "Banana;Mango;manzana"
        ///
        /// Ejemplo 3:
        /// string unico = "ElementoUnico".OrderStringAscBySeparator(',');
        /// // Resultado: "ElementoUnico"
        /// </example>
        public static string OrderStringAscBySeparator(this string strUnordered, char parameters)
        {
            if (!strUnordered.HasItems())
            {
                return string.Empty;
            }

            if (parameters.IsNull())
            {
                return string.Empty;
            }

            var arrayOfComponents = strUnordered.Split(parameters);


            if (arrayOfComponents.First() == strUnordered)
            {
                return strUnordered;
            }

            var elementsOfArray = arrayOfComponents.Count();

            if (elementsOfArray > 1)
            {
                var orderedListOfComponents = arrayOfComponents.OrderBy(e => e.ToUpper());
                var stringOrdered = string.Join(parameters.ToString(), orderedListOfComponents);

                return stringOrdered;
            }

            return strUnordered;
        }
    }
}
