using System;

namespace JiraWorklogsApp.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateRange ThisYear(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, 1, 1);
            range.End = range.Start.Value.AddYears(1).AddSeconds(-1);

            return range;
        }

        public static DateRange LastYear(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year - 1, 1, 1);
            range.End = range.Start.Value.AddYears(1).AddSeconds(-1);

            return range;
        }

        public static DateRange ThisMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = new DateTime(date.Year, date.Month, 1);
            range.End = range.Start.Value.AddMonths(1).AddSeconds(-1);

            return range;
        }

        public static DateRange LastMonth(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = (new DateTime(date.Year, date.Month, 1)).AddMonths(-1);
            range.End = range.Start.Value.AddMonths(1).AddSeconds(-1);

            return range;
        }

        public static DateRange ThisWeek(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = date.Date.AddDays(-GetOffsetToTheBegWeek(date));
            range.End = range.Start.Value.AddDays(7).AddSeconds(-1);

            return range;
        }

        public static DateRange LastWeek(DateTime date)
        {
            DateRange range = ThisWeek(date);

            if (range.Start != null)
                range.Start = range.Start.Value.AddDays(-7);
            if (range.End != null)
                range.End = range.End.Value.AddDays(-7);

            return range;
        }

        public static DateRange ThisDay(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = date.Date;
            range.End = range.Start.Value.AddDays(1).AddSeconds(-1);

            return range;
        }

        private static int GetOffsetToTheBegWeek(DateTime dt)
        {
            var dofweek = dt.DayOfWeek;

            var m = 0;

            switch (dofweek)
            {
                case DayOfWeek.Monday:
                    m = 0;
                    break;
                case DayOfWeek.Tuesday:
                    m = 1;
                    break;
                case DayOfWeek.Wednesday:
                    m = 2;
                    break;
                case DayOfWeek.Thursday:
                    m = 3;
                    break;
                case DayOfWeek.Friday:
                    m = 4;
                    break;
                case DayOfWeek.Saturday:
                    m = 5;
                    break;
                case DayOfWeek.Sunday:
                    m = 6;
                    break;
            }

            return m;
        }
    }
}
