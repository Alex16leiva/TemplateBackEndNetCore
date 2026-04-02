using System.Globalization;

namespace Dominio.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Devuelve una representación en cadena de un objeto <see cref="DateTime"/> 
        /// con el formato "yyyy-MMM-dd hh:mm tt".
        /// </summary>
        /// <param name="date">La fecha y hora que se desea formatear.</param>
        /// <returns>
        /// Una cadena que representa la fecha y hora en el formato:
        /// Año-Mes abreviado-Día Hora:minutos AM/PM.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime fechaActual = DateTime.Now;
        /// string resultado = fechaActual.WeekDateName();
        /// Console.WriteLine(resultado);
        /// // Salida posible: "2026-Apr-02 02:50 PM"
        /// </code>
        /// </example>
        public static string WeekDateName(this DateTime date)
        {
            return date.ToString("yyyy-MMM-dd hh:mm tt");
        }

        /// <summary>
        /// Devuelve la fecha mínima (más antigua) de una colección de objetos <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dates">La colección de fechas de la cual se obtendrá la mínima.</param>
        /// <returns>
        /// El valor <see cref="DateTime"/> más pequeño dentro de la colección.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Se produce si la colección está vacía.
        /// </exception>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// List<DateTime> fechas = new List<DateTime>
        /// {
        ///     new DateTime(2026, 4, 2),
        ///     new DateTime(2025, 12, 25),
        ///     new DateTime(2026, 1, 1)
        /// };
        ///
        /// DateTime fechaMinima = fechas.MinDate();
        /// Console.WriteLine(fechaMinima);
        /// // Salida: 25/12/2025
        /// </code>
        /// </example>
        public static DateTime MinDate(this IEnumerable<DateTime> dates)
        {
            return dates.Items().Min(c => c);
        }

        /// <summary>
        /// Devuelve la fecha máxima (más reciente) de una colección de objetos <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dates">La colección de fechas de la cual se obtendrá la máxima.</param>
        /// <returns>
        /// El valor <see cref="DateTime"/> más grande dentro de la colección.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Se produce si la colección está vacía.
        /// </exception>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// List<DateTime> fechas = new List<DateTime>
        /// {
        ///     new DateTime(2026, 4, 2),
        ///     new DateTime(2025, 12, 25),
        ///     new DateTime(2026, 1, 1)
        /// };
        ///
        /// DateTime fechaMaxima = fechas.MaxDate();
        /// Console.WriteLine(fechaMaxima);
        /// // Salida: 02/04/2026
        /// </code>
        /// </example>
        public static DateTime MaxDate(this IEnumerable<DateTime> dates)
        {
            return dates.Items().Max(c => c);
        }

        /// <summary>
        /// Determina si una fecha ocurre antes de otra fecha dada.
        /// </summary>
        /// <param name="date">La fecha que se desea evaluar.</param>
        /// <param name="startDate">La fecha de referencia para comparar.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="date"/> ocurre antes de <paramref name="startDate"/>; 
        /// en caso contrario, <c>false</c>. 
        /// También devuelve <c>false</c> si cualquiera de las fechas es nula.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime? fechaEvento = new DateTime(2025, 12, 25);
        /// DateTime? fechaReferencia = new DateTime(2026, 1, 1);
        ///
        /// bool ocurreAntes = fechaEvento.OccursBefore(fechaReferencia);
        /// Console.WriteLine(ocurreAntes);
        /// // Salida: True (porque 25/12/2025 ocurre antes de 01/01/2026)
        /// </code>
        /// </example>
        public static bool OccursBefore(this DateTime? date, DateTime? startDate)
        {
            if (date.IsNull() || startDate.IsNull()) return false;

            return startDate.Value.Ticks > date.Value.Ticks;
        }

        /// <summary>
        /// Determina si una fecha se encuentra dentro de un rango específico.
        /// </summary>
        /// <param name="date">La fecha que se desea evaluar.</param>
        /// <param name="startDate">La fecha inicial del rango.</param>
        /// <param name="endDate">La fecha final del rango.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="date"/> está entre <paramref name="startDate"/> 
        /// (inclusive) y <paramref name="endDate"/> (exclusiva); en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime fecha = new DateTime(2026, 4, 2);
        /// DateTime inicio = new DateTime(2026, 4, 1);
        /// DateTime fin = new DateTime(2026, 4, 10);
        ///
        /// bool dentroDelRango = fecha.Between(inicio, fin);
        /// Console.WriteLine(dentroDelRango);
        /// // Salida: True (porque 02/04/2026 está entre 01/04/2026 y 10/04/2026)
        /// </code>
        /// </example>
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            if (date.IsNull() || startDate.IsNull()) return false;

            return date.Ticks >= startDate.Ticks && date.Ticks < endDate.Ticks;
        }

        /// <summary>
        /// Obtiene el número de semana del año para una fecha determinada,
        /// utilizando la cultura actual del sistema.
        /// </summary>
        /// <param name="date">La fecha de la cual se desea obtener el número de semana.</param>
        /// <returns>
        /// Un entero que representa el número de semana del año en el que cae la fecha.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime fecha = new DateTime(2026, 4, 2);
        /// int numeroSemana = fecha.GetWeekNumber();
        /// Console.WriteLine(numeroSemana);
        /// // Salida posible: 14 (dependiendo de la configuración cultural del sistema)
        /// </code>
        /// </example>
        public static int GetWeekNumber(this DateTime date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;

            return cul.Calendar.GetWeekOfYear(
                 date,
                 CalendarWeekRule.FirstDay,
                 DayOfWeek.Sunday);
        }

        /// <summary>
        /// Obtiene el número de la última semana del año actual.
        /// </summary>
        /// <param name="date">
        /// La fecha de referencia (no se utiliza directamente, ya que se reemplaza por el 31 de diciembre del año actual).
        /// </param>
        /// <returns>
        /// Un entero que representa el número de la última semana del año actual.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime fecha = DateTime.Now;
        /// int ultimaSemana = fecha.GetLastWeekNumberCurrentYear();
        /// Console.WriteLine(ultimaSemana);
        /// // Salida posible: 52 o 53 (dependiendo de cómo se calculen las semanas en la cultura actual)
        /// </code>
        /// </example>
        public static int GetLastWeekNumberCurrentYear(this DateTime date)
        {
            date = new DateTime(DateTime.Now.Year, 12, 31);
            return date.GetWeekNumber();
        }

        /// <summary>
        /// Determina si un objeto <see cref="DateTime"/> tiene el valor por defecto.
        /// </summary>
        /// <param name="dateTime">La fecha que se desea evaluar.</param>
        /// <returns>
        /// <c>true</c> si <paramref name="dateTime"/> es igual a <c>default(DateTime)</c>
        /// (01/01/0001 00:00:00); en caso contrario, <c>false</c>.
        /// </returns>
        /// <example>
        /// Ejemplo de uso:
        /// <code>
        /// DateTime fecha1 = default(DateTime);
        /// DateTime fecha2 = DateTime.Now;
        ///
        /// bool esDefault1 = fecha1.HasDefaultValue(); // True
        /// bool esDefault2 = fecha2.HasDefaultValue(); // False
        ///
        /// Console.WriteLine($"Fecha1 es default: {esDefault1}");
        /// Console.WriteLine($"Fecha2 es default: {esDefault2}");
        /// </code>
        /// </example>
        public static bool HasDefaultValue(this DateTime dateTime)
        {
            return dateTime == default(DateTime);
        }
    }
}
