using System;

namespace Barcodes.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToExpireDate(this DateTime dateTime, bool emptyDay)
        {
            return dateTime.ToString(emptyDay ? "yyMM00" : "yyMMdd");
        }
    }
}
