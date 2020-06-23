namespace JiraWorklogsApp.Common.Models.Params
{
    public class GetAssignableUsersParams
    {
        public string ProjectKey { get; set; }

        public JiraConnectionShortInfo JiraConnection { get; set; }
    }
}
