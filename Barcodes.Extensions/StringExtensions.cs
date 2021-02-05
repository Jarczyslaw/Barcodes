using System.Text.RegularExpressions;

namespace Barcodes.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsOnlyDigits(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        public static string Sanitize(this string input, int maxLength = int.MaxValue)
        {
            var result = Regex.Replace(input, @"\t|\n|\r", " ");
            result = Regex.Replace(result, @"\p{C}+", string.Empty);
            if (result.Length > maxLength)
            {
                result = result.Substring(0, maxLength) + "...";
            }
            return result;
        }
    }
}
