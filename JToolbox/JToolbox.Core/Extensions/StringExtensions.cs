using System;
using System.Linq;

namespace JToolbox.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int length)
        {
            return value.Substring(0, Math.Min(value.Length, length));
        }

        public static bool IgnoreCaseContains(this string val1, string val2)
        {
            return val1.IndexOf(val2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IgnoreCaseEquals(this string val1, string val2)
        {
            return val1.Equals(val2, StringComparison.OrdinalIgnoreCase);
        }

        public static string ExtractBetween(this string @string, string from, string to)
        {
            var indexFrom = @string.IndexOf(from);
            if (indexFrom < 0)
            {
                return string.Empty;
            }
            indexFrom += from.Length;

            var indexTo = @string.LastIndexOf(to);
            if (indexTo < 0)
            {
                return string.Empty;
            }

            return @string.Substring(indexFrom, indexTo - indexFrom);
        }

        public static string OnlyDigits(this string @this)
        {
            var result = string.Empty;
            for (int i = 0; i < @this.Length; i++)
            {
                if (char.IsDigit(@this[i]))
                {
                    result += @this[i];
                }
            }
            return result;
        }

        public static string WithoutWhitespaces(this string @this)
        {
            return new string(@this.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}