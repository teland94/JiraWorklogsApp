namespace JiraWorklogsApp.Common.Models
{
    public class JiraProject
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public JiraConnectionShortInfo JiraConnection { get; set; }
    }
}
