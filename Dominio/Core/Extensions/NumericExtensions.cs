namespace Dominio.Core.Extensions
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Convierte un número de tipo <see cref="double"/> en su valor absoluto
        /// y lo transforma en un entero.
        /// </summary>
        /// <param name="number">El número de tipo <see cref="double"/> que se desea convertir.</param>
        /// <returns>
        /// El valor absoluto del número convertido a <see cref="int"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// double valor1 = -12.7;
        /// double valor2 = 8.9;
        ///
        /// int resultado1 = valor1.ToAbsInt(); // 12
        /// int resultado2 = valor2.ToAbsInt(); // 8
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static int ToAbsInt(this double number)
        {
            return (int)Abs(number);
        }

        /// <summary>
        /// Convierte un número de tipo <see cref="double"/> en un entero,
        /// aplicando la función de techo (<see cref="Math.Ceiling"/>).
        /// </summary>
        /// <param name="number">El número de tipo <see cref="double"/> que se desea convertir.</param>
        /// <returns>
        /// El valor entero resultante de aplicar <see cref="Math.Ceiling"/> al número.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// double valor1 = 12.3;
        /// double valor2 = -7.8;
        ///
        /// int resultado1 = valor1.ToInt(); // 13
        /// int resultado2 = valor2.ToInt(); // -7
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static int ToInt(this double number)
        {
            return Convert.ToInt32(Math.Ceiling(number));
        }

        /// <summary>
        /// Devuelve el valor absoluto de un número entero.
        /// </summary>
        /// <param name="number">El número entero que se desea evaluar.</param>
        /// <returns>
        /// El valor absoluto de <paramref name="number"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// int valor1 = -15;
        /// int valor2 = 20;
        ///
        /// int resultado1 = valor1.Abs(); // 15
        /// int resultado2 = valor2.Abs(); // 20
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static int Abs(this int number)
        {
            return Math.Abs(number);
        }

        /// <summary>
        /// Devuelve el valor absoluto de un número decimal.
        /// </summary>
        /// <param name="number">El número decimal que se desea evaluar.</param>
        /// <returns>
        /// El valor absoluto de <paramref name="number"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal valor1 = -15.75m;
        /// decimal valor2 = 20.10m;
        ///
        /// decimal resultado1 = valor1.Abs(); // 15.75
        /// decimal resultado2 = valor2.Abs(); // 20.10
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static decimal Abs(this decimal number)
        {
            return Math.Abs(number);
        }

        /// <summary>
        /// Devuelve el valor absoluto de un número de tipo <see cref="double"/>.
        /// </summary>
        /// <param name="number">El número de tipo <see cref="double"/> que se desea evaluar.</param>
        /// <returns>
        /// El valor absoluto de <paramref name="number"/>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// double valor1 = -12.5;
        /// double valor2 = 9.3;
        ///
        /// double resultado1 = valor1.Abs(); // 12.5
        /// double resultado2 = valor2.Abs(); // 9.3
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static double Abs(this double number)
        {
            return Math.Abs(number);
        }

        /// <summary>
        /// Redondea un número decimal a dos decimales,
        /// útil para representar valores monetarios.
        /// </summary>
        /// <param name="number">El número decimal que se desea redondear.</param>
        /// <returns>
        /// El valor de <paramref name="number"/> redondeado a dos decimales.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal precio1 = 123.456m;
        /// decimal precio2 = -78.987m;
        ///
        /// decimal resultado1 = precio1.AsMoney(); // 123.46
        /// decimal resultado2 = precio2.AsMoney(); // -78.99
        ///
        /// Console.WriteLine($"Precio1: {resultado1}");
        /// Console.WriteLine($"Precio2: {resultado2}");
        /// </code>
        /// </example>
        public static decimal AsMoney(this decimal number)
        {
            return Math.Round(number, 2);
        }

        /// <summary>
        /// Convierte un número decimal en su representación porcentual,
        /// dividiéndolo entre 100.
        /// </summary>
        /// <param name="number">El número decimal que se desea convertir.</param>
        /// <returns>
        /// El valor de <paramref name="number"/> dividido entre 100.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal valor1 = 75m;
        /// decimal valor2 = 12.5m;
        ///
        /// decimal resultado1 = valor1.AsDecimalPercentage(); // 0.75
        /// decimal resultado2 = valor2.AsDecimalPercentage(); // 0.125
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static decimal AsDecimalPercentage(this decimal number)
        {
            return number / 100;
        }

        /// <summary>
        /// Convierte un valor decimal que representa minutos en su equivalente en horas.
        /// </summary>
        /// <param name="value">El valor en minutos que se desea convertir.</param>
        /// <returns>
        /// El valor en horas, calculado como <paramref name="value"/> dividido entre 60.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal minutos1 = 120m;
        /// decimal minutos2 = 90m;
        ///
        /// decimal horas1 = minutos1.ToHours(); // 2
        /// decimal horas2 = minutos2.ToHours(); // 1.5
        ///
        /// Console.WriteLine($"Horas1: {horas1}");
        /// Console.WriteLine($"Horas2: {horas2}");
        /// </code>
        /// </example>
        public static decimal ToHours(this decimal value)
        {
            return value / 60;
        }

        /// <summary>
        /// Ajusta un número decimal para que tenga exactamente la cantidad de decimales especificada,
        /// rellenando con ceros a la derecha si es necesario.
        /// </summary>
        /// <param name="number">El número decimal que se desea ajustar.</param>
        /// <param name="decimalPlaces">La cantidad de posiciones decimales que debe tener el número.</param>
        /// <returns>
        /// El número ajustado con exactamente <paramref name="decimalPlaces"/> decimales.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal valor1 = 12.3m;
        /// decimal valor2 = 45m;
        ///
        /// decimal resultado1 = valor1.PadRightDecimal(4); // 12.3000
        /// decimal resultado2 = valor2.PadRightDecimal(2); // 45.00
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static decimal PadRightDecimal(this decimal number, int decimalPlaces)
        {
            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
            var stringNumber = number.ToString();

            string decimalStringNumber = regex.IsMatch(stringNumber) ? regex.Match(stringNumber).Value : "0";
            decimalStringNumber = decimalStringNumber.PadRight(decimalPlaces, '0');

            var decimaNumber = Math.Round(Convert.ToDecimal($".{decimalStringNumber}"), decimalPlaces);

            return Math.Truncate(number) + decimaNumber;
        }

        /// <summary>
        /// Convierte un valor <see cref="decimal?"/> en <see cref="decimal"/>,
        /// devolviendo 0 si el valor es nulo.
        /// </summary>
        /// <param name="number">El valor decimal nullable que se desea evaluar.</param>
        /// <returns>
        /// El valor de <paramref name="number"/> si no es nulo; en caso contrario, 0.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// decimal? valor1 = null;
        /// decimal? valor2 = 25.75m;
        ///
        /// decimal resultado1 = valor1.DecimalValue(); // 0
        /// decimal resultado2 = valor2.DecimalValue(); // 25.75
        ///
        /// Console.WriteLine($"Resultado1: {resultado1}");
        /// Console.WriteLine($"Resultado2: {resultado2}");
        /// </code>
        /// </example>
        public static decimal DecimalValue(this decimal? number)
        {
            return number.IsNull() ? 0 : number.Value;
        }
    }
}
