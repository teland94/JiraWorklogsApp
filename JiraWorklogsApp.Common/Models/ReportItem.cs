using System;
using JiraWorklogsApp.Common.Attributes;

namespace JiraWorklogsApp.Common.Models
{
    public class ReportItem
    {
        [ExportToExcel(0, "Date")]
        public DateTime Date { get; set; }

        [ExportToExcel(1, "Project Name")]
        public string ProjectName { get; set; }

        [ExportToExcel(2, "User Name")]
        public string UserName { get; set; }

        [ExportToExcel(3, "Issue Key")]
        public string IssueKey { get; set; }

        [ExportToExcel(4, "Issue Title")]
        public string IssueTitle { get; set; }

        [ExportToExcel(6, "Worklog Description")]
        public string WorklogDescription { get; set; }

        [ExportToExcel(7, "Hours")]
        public decimal Hours { get; set; }

        [ExportToExcel(5, "Estimate")]
        public decimal? StoryPointEstimate { get; set; }
    }
}