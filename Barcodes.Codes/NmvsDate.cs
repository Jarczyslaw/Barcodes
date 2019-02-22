using System;

namespace Barcodes.Codes
{
    public class NmvsDate : IEquatable<NmvsDate>
    {
        public NmvsDate(string value)
            : this(Parse(value))
        {
        }

        public NmvsDate(int year, int month, int day)
        {
            CheckValues(year, month, day);
            Year = year;
            Month = month;
            Day = day;
        }

        public NmvsDate(DateTime dateTime)
            : this(dateTime.Year, dateTime.Month, dateTime.Day)
        {
        }

        public NmvsDate(NmvsDate nmvsDate)
        {
            Year = nmvsDate.Year;
            Month = nmvsDate.Month;
            Day = nmvsDate.Day;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public static NmvsDate Parse(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 6)
            {
                throw new ArgumentException("Invalid nmvs date value");
            }

            var yearValue = value.Substring(0, 2);
            var monthValue = value.Substring(2, 2);
            var dayValue = value.Substring(4, 2);

            int year = int.Parse(yearValue);
            year += DateTime.Now.Year - (DateTime.Now.Year % 100);
            int month = int.Parse(monthValue);
            int day = int.Parse(dayValue);

            CheckValues(year, month, day);
            return new NmvsDate(year, month, day);
        }

        public static bool TryParse(string value, out NmvsDate nmvsDate)
        {
            nmvsDate = null;
            try
            {
                nmvsDate = Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Equals(NmvsDate other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day == 0 ? DateTime.DaysInMonth(Year, Month) : Day);
        }

        public override string ToString()
        {
            return $"{Year % 100:00}{Month:00}{Day:00}";
        }

        private static void CheckValues(int year, int month, int day)
        {
            if (day == 0)
            {
                day = DateTime.DaysInMonth(year, month);
            }
            new DateTime(year, month, day);
        }
    }
}
