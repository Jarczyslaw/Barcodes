using System;

namespace JToolbox.Core.Utilities
{
    public static class QuarterDates
    {
        public static int GetQuarter(DateTime date)
        {
            return GetQuarter(date.Month);
        }

        public static int GetQuarter(int month)
        {
            return (month + 2) / 3;
        }

        public static DateTime QuarterStartDate(DateTime date)
        {
            return QuarterStartDate(date.Year, GetQuarter(date));
        }

        public static DateTime QuarterStartDate(int year, int quarter)
        {
            return new DateTime(year, quarter * 3 - 2, 1);
        }

        public static DateTime QuarterEndDate(DateTime date)
        {
            return QuarterEndDate(date.Year, GetQuarter(date));
        }

        public static DateTime QuarterEndDate(int year, int quarter)
        {
            var month = quarter * 3;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month))
                .AddDays(1)
                .AddMilliseconds(-1);
        }

        public static int MonthInQuarter(int month)
        {
            return (month - 1) % 3 + 1;
        }

        public static int MonthInQuarter(DateTime date)
        {
            return MonthInQuarter(date.Month);
        }
    }
}