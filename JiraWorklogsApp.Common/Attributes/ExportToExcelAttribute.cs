using System;

namespace JiraWorklogsApp.Common.Attributes
{
    public class ExportToExcelAttribute : Attribute
    {
        public int Column { get; set; }
        public string Title { get; set; }

        public ExportToExcelAttribute(int column, string title)
        {
            Column = column;
            Title = title;
        }
    }
}
