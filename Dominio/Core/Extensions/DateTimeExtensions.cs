using System.Globalization;

namespace Dominio.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string WeekDateName(this DateTime date)
        {
            return date.ToString("yyyy-MMM-dd hh:mm tt");
        }
        public static DateTime MinDate(this IEnumerable<DateTime> dates)
        {
            return dates.Items().Min(c => c);
        }
        public static DateTime MaxDate(this IEnumerable<DateTime> dates)
        {
            return dates.Items().Max(c => c);
        }
        public static bool OccursBefore(this DateTime? date, DateTime? startDate)
        {
            if (date.IsNull() || startDate.IsNull()) return false;

            return startDate.Value.Ticks > date.Value.Ticks;
        }
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            if (date.IsNull() || startDate.IsNull()) return false;

            return date.Ticks >= startDate.Ticks && date.Ticks < endDate.Ticks;
        }

        public static int GetWeekNumber(this DateTime date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;

            return cul.Calendar.GetWeekOfYear(
                 date,
                 CalendarWeekRule.FirstDay,
                 DayOfWeek.Sunday);
        }

        public static int GetLastWeekNumberCurrentYear(this DateTime date)
        {
            date = new DateTime(DateTime.Now.Year, 12, 31);
            return date.GetWeekNumber();

        }
        public static bool HasDefaultValue(this DateTime dateTime)
        {
            return dateTime == default(DateTime);
        }
    }
}
