using System;

namespace JiraWorklogsApp.Common.Helpers
{
    public struct DateRange
    {
        public DateRange(DateTime? start, DateTime? end) : this()
        {
            Start = start;
            End = end;
        }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
