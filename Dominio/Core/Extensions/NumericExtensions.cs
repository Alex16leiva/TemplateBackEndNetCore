namespace Dominio.Core.Extensions
{
    public static class NumericExtensions
    {
        public static int ToAbsInt(this double number)
        {
            return (int)Abs(number);
        }

        public static int ToInt(this double number)
        {
            return Convert.ToInt32(Math.Ceiling(number));
        }

        public static int Abs(this int number)
        {
            return Math.Abs(number);
        }

        public static decimal Abs(this decimal number)
        {
            return Math.Abs(number);
        }

        public static double Abs(this double number)
        {
            return Math.Abs(number);
        }

        public static decimal AsMoney(this decimal number)
        {
            return Math.Round(number, 2);
        }

        public static decimal AsDecimalPercentage(this decimal number)
        {
            return number / 100;
        }

        public static decimal ToHours(this decimal value)
        {
            return value / 60;
        }
        public static decimal PadRightDecimal(this decimal number, int decimalPlaces)
        {
            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
            var stringNumber = number.ToString();

            string decimalStringNumber = regex.IsMatch(stringNumber) ? regex.Match(stringNumber).Value : "0";
            decimalStringNumber = decimalStringNumber.PadRight(decimalPlaces, '0');

            var decimaNumber = Math.Round(Convert.ToDecimal($".{decimalStringNumber}"), decimalPlaces);

            return Math.Truncate(number) + decimaNumber;
        }

        public static decimal DecimalValue(this decimal? number)
        {
            return number.IsNull() ? 0 : number.Value;
        }
    }
}
