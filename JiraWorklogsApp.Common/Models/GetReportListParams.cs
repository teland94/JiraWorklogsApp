using JiraWorklogsApp.Common.Helpers;

namespace JiraWorklogsApp.Common.Models
{
    public class GetReportListParams
    {
        public DateRange DateRange { get; set; }
        public string ProjectKey { get; set; }

        public JiraConnectionShortInfo JiraConnection { get; set; }
    }
}
