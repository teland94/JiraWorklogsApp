using JiraWorklogsApp.Common.Helpers;

namespace JiraWorklogsApp.Common.Models.Params
{
    public class GetReportListParams
    {
        public DateRange DateRange { get; set; }
        public string ProjectKey { get; set; }
        public string UserName { get; set; }

        public int TimezoneOffset { get; set; }

        public JiraConnectionShortInfo JiraConnection { get; set; }
    }
}
