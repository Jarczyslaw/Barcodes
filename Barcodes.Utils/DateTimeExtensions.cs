using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToExpireDate(this DateTime dateTime, bool emptyDay)
        {
            return dateTime.ToString(emptyDay ? "yyMM00" : "yyMMdd");
        }
    }
}
