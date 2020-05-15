using System;

namespace Barcodes.Codes
{
    public class NmvsDate : IEquatable<NmvsDate>
    {
        public NmvsDate(string value)
        {
            Parse(value);
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

        public int Year { get; private set; } = 1;
        public int Month { get; private set; } = 1;
        public int Day { get; private set; } = 1;

        public void Parse(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 6)
            {
                throw new ArgumentException("Invalid value length");
            }

            var yearValue = value.Substring(0, 2);
            var monthValue = value.Substring(2, 2);
            var dayValue = value.Substring(4, 2);

            if (!int.TryParse(yearValue, out int year))
            {
                throw new ArgumentException("Invalid year value");
            }
            year = DateTime.Now.Year / 100 + year;

            if (!int.TryParse(monthValue, out int month))
            {
                throw new ArgumentException("Invalid month value");
            }

            if (!int.TryParse(dayValue, out int day))
            {
                throw new ArgumentException("Invalid day value");
            }

            CheckValues(year, month, day);

            Year = year;
            Month = month;
            Day = day;
        }

        public static bool TryParse(string value, out NmvsDate nmvsDate)
        {
            nmvsDate = null;
            try
            {
                nmvsDate = new NmvsDate(value);
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

        private void CheckValues(int year, int month, int day)
        {
            try
            {
                var nmvsDay = day;
                if (day == 0)
                {
                    nmvsDay = DateTime.DaysInMonth(year, month);
                }
                new DateTime(year, month, nmvsDay);
            }
            catch
            {
                throw new ArgumentException("Invalid date values");
            }
        }

        public override string ToString()
        {
            return $"{Year % 100:00}{Month:00}{Day:00}";
        }
    }
}
