using System.Text.RegularExpressions;

namespace Dominio.Core.Extensions
{
    public static class StringExtensions
    {
        public static object[] ToObject(this string value)
        {
            return new object[] { value.ValueOrEmpty() };
        }
        public static decimal ToDecimal(this string decimalStringValue)
        {
            decimal decimalValue = 0;
            if (decimalStringValue.HasValue())
            {
                Decimal.TryParse(decimalStringValue, out decimalValue);
            }
            return decimalValue;
        }

        public static int ToInt(this string decimalStringValue)
        {
            int decimalValue = 0;
            if (decimalStringValue.HasValue())
            {
                Int32.TryParse(decimalStringValue, out decimalValue);
            }
            return decimalValue;
        }

        public static string GetNumericValues(this string decimalStringValue)
        {
            string output = string.Empty;
            if (decimalStringValue.HasValue())
            {
                output = new string(decimalStringValue.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            }
            return output;
        }

        public static bool HasValue(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue) && !string.IsNullOrWhiteSpace(stringValue);
        }
        public static string ValueOrEmpty(this string stringValue)
        {
            return HasValue(stringValue) ? stringValue.Trim() : string.Empty;
        }

        public static string ToStringValue(this object stringValue)
        {
            return stringValue != null ? stringValue.ToString().Trim() : string.Empty;
        }

        public static bool IsMissingValue(this string stringValue)
        {
            return !HasValue(stringValue);
        }

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

        public static DateTime ToDateTime(this string dateStringValue)
        {
            DateTime dateValue = DateTime.MinValue;
            if (dateStringValue.HasValue())
            {
                DateTime.TryParse(dateStringValue, out dateValue);
            }
            return dateValue;
        }

        public static string AddSpacesBeforeCapitalLetters(this string value)
        {
            return string.Concat(value.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
        public static bool IsNumeric(this string str)
        {
            if (str.HasValue())
            {
                return !str.ToArray().Any(a => !char.IsDigit(a));
            }
            return true;
        }
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

        public static string GetBatchNoFromProcessSheetId(this string processSheetId)
        {
            if (processSheetId.Contains("-"))
            {
                return processSheetId.Substring(0, processSheetId.Length - 2);
            }
            return processSheetId;
        }

        public static bool IsOpenClothType(this string sku)
        {
            if (sku.IsMissingValue())
            {
                throw new ArgumentNullException("Define Sku");
            }

            decimal final = sku.GetWidthFC();
            return final >= 48;
        }
        public static decimal GetWidthFC(this string sku)
        {
            if (sku.IsMissingValue())
            {
                throw new ArgumentNullException("Define Sku");
            }

            var skuFragment = sku.Split('.');

            if (skuFragment.Length < 3)
            {

                throw new ArgumentNullException("Invalid Sku");
            }

            var width = skuFragment[3];
            if (width.Contains("-"))
            {
                return Convert.ToDecimal(width.Replace("-", "."));
            }

            return Convert.ToDecimal(GetNumericValues(width));
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string RemoveSpaceEmpty(this string str)
        {
            return Regex.Replace(str, @"\s", "")?.Trim();
        }

        public static string AddStringParameters(this string str, object[] parameters)
        {
            return string.Format(str, parameters);
        }

        /// <summary>
        /// The generated string of for example if sent a parameter: ',' and strUnordered: "C,B,a"
        /// the result string will be "A,B,C", every character in upper case as well,
        /// if one of the both variables are null or empty, it will return string.empty
        /// </summary>
        /// <param name="strUnordered">The string to be ordered Ascendent, separated by the delimeter</param>
        /// <param name="parameters">The separator or delimeter. </param>
        /// <returns></returns>
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
