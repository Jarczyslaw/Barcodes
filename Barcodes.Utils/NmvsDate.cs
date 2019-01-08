using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Utils
{
    public class NmvsDate
    {
        public NmvsDate(string value)
        {
            if (!TryParse(value, out NmvsDate date))
            {
                throw new ArgumentException("Incorrect NmvsDate string format. Correct format is yyMMdd or yyMM00");
            }

            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        public NmvsDate(int value)
            : this(value.ToString())
        {

        }

        public NmvsDate(int year, int month, int day)
        {
            ParseData(year, month, day, true);

            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public static bool ParseData(int year, int month, int day, bool throwException = false)
        {
            try
            {
                var dt = new DateTime(year, month, day == 0 ? 1 : day);
                return true;
            }
            catch
            {
                if (throwException)
                    throw;
                return false;
            }
        }

        public static bool TryParse(string value, out NmvsDate date)
        {
            date = null;

            if (string.IsNullOrEmpty(value) || value.Length != 6)
            {
                return false;
            }

            var yearValue = value.Substring(0, 2);
            var monthValue = value.Substring(2, 2);
            var dayValue = value.Substring(4, 2);

            try
            {
                int year = int.Parse(yearValue);
                year += DateTime.Now.Year / 1000 * 1000;
                int month = int.Parse(monthValue);
                int day = int.Parse(dayValue);

                if (ParseData(year, month, day))
                {
                    date = new NmvsDate(year, month, day);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Year % 100:00}{Month:00}{Day:00}";
        }
    }
}
