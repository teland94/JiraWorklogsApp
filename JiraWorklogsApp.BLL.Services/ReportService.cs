using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapplo.Jira;
using Dapplo.Jira.Entities;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Helpers;
using JiraWorklogsApp.Common.Models;
using JiraWorklogsApp.Common.Models.Params;

namespace JiraWorklogsApp.BLL.Services
{
    public class ReportService : IReportService
    {
        private IJiraClient JiraClient { get; set; }
        private IJiraConnectionsService JiraConnectionService { get; }
        private ITempoService TempoService { get; }

        public ReportService(IJiraConnectionsService jiraConnectionService)
        {
            TempoService = Services.TempoService.Create();
            JiraConnectionService = jiraConnectionService;
        }
        
        public async Task<IEnumerable<JiraProject>> GetProjectsAsync(ICollection<JiraConnectionShortInfo> jiraConnections, string userId)
        {
            var connections = await JiraConnectionService.GetAsync(userId);

            var projects = new List<JiraProject>();

            foreach (var connection in connections)
            {
                var token = string.IsNullOrWhiteSpace(connection.AuthToken) ? jiraConnections.FirstOrDefault(c => c.Id == connection.Id)?.AuthToken : connection?.AuthToken;
                JiraClient = Dapplo.Jira.JiraClient.Create(new Uri(connection.InstanceUrl));
                JiraClient.SetBasicAuthentication(connection.UserName, token);

                projects.AddRange((await JiraClient.Project.GetAllAsync()).Select(p => new JiraProject
                {
                    Id = p.Id,
                    Key = p.Key,
                    Name = p.Name,
                    JiraConnection = new JiraConnectionShortInfo
                    {
                        Id = connection.Id
                    }
                }));
            }

            return projects;
        }

        public async Task<IEnumerable<JiraUser>> GetAssignableUsers(GetAssignableUsersParams getAssignableUsersParams)
        {
            var connection = await JiraConnectionService.GetAsync(getAssignableUsersParams.JiraConnection.Id);
            var token = string.IsNullOrWhiteSpace(connection.AuthToken) ? getAssignableUsersParams.JiraConnection.AuthToken : connection.AuthToken;
            JiraClient = Dapplo.Jira.JiraClient.Create(new Uri(connection.InstanceUrl));
            JiraClient.SetBasicAuthentication(connection.UserName, token);

            var users = await JiraClient.User.GetAssignableUsersAsync(projectKey: getAssignableUsersParams.ProjectKey);

            return users.Select(u => new JiraUser
            {
                EmailAddress = u.EmailAddress,
                DisplayName = u.DisplayName
            });
        }

        public async Task<IEnumerable<ReportItem>> GetReportListAsync(GetReportListParams getReportListParams)
        {
            return await GetReportDataListAsync(getReportListParams);
        }

        public async Task<byte[]> GetReportExcelDataAsync(GetReportListParams getReportListParams)
        {
            var reportItems = await GetReportDataListAsync(getReportListParams);

            reportItems = reportItems.OrderBy(d => d.Date).ToList();

            foreach (var reportItem in reportItems)
            {
                reportItem.Date = reportItem.Date.AddHours(getReportListParams.TimezoneOffset);
            }

            return ExcelHelper.GetExportFile(reportItems, "Report");
        }

        private async Task<List<ReportItem>> GetReportDataListAsync(GetReportListParams getReportListParams)
        {
            var connection = await JiraConnectionService.GetAsync(getReportListParams.JiraConnection.Id);
            List<ReportItem> reportDataList;

            if (string.IsNullOrWhiteSpace(connection.TempoAuthToken) &&
                string.IsNullOrWhiteSpace(getReportListParams.JiraConnection.TempoAuthToken))
            {
                var token = string.IsNullOrWhiteSpace(connection.AuthToken) ? getReportListParams.JiraConnection.AuthToken : connection.AuthToken;
                JiraClient = Dapplo.Jira.JiraClient.Create(new Uri(connection.InstanceUrl));
                JiraClient.SetBasicAuthentication(connection.UserName, token);
                reportDataList = await GetJiraReportDataList(getReportListParams);
            }
            else
            {
                var token = string.IsNullOrWhiteSpace(connection.TempoAuthToken) ? getReportListParams.JiraConnection.TempoAuthToken : connection.TempoAuthToken;
                TempoService.SetBaseUrl(connection.InstanceUrl);
                TempoService.SetTempoApiToken(token);

                reportDataList = await GetTempoReportDataList(getReportListParams);
            }

            reportDataList = reportDataList.OrderByDescending(d => d.Date).ToList();

            return reportDataList;
        }

        private async Task<List<ReportItem>> GetJiraReportDataList(GetReportListParams getReportListParams)
        {
            var startDate = getReportListParams.DateRange.Start != null 
                ? getReportListParams.DateRange.Start.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) 
                : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDate = getReportListParams.DateRange.End != null
                ? getReportListParams.DateRange.End.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) 
                : DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var jqlStr = new StringBuilder($"project='{getReportListParams.ProjectKey}'" +
                                           $"AND worklogDate >= '{startDate}' AND worklogDate <= '{endDate}'");
            if (!string.IsNullOrEmpty(getReportListParams.UserName))
            {
                jqlStr.Append($"AND worklogAuthor = '{getReportListParams.UserName}'");
            }
            var issueSearch = new JqlIssueSearch { Jql = jqlStr.ToString() };

            var searchFields = new List<string>(JiraConfig.SearchFields);

            var fields = await JiraClient.Server.GetFieldsAsync();
            var storyPointEstimateCustomField = fields.FirstOrDefault(f => f.Name == "Story point estimate" && f.IsCustom);
            if (storyPointEstimateCustomField != null)
            {
                searchFields.Add(storyPointEstimateCustomField.Id);
            }

            var issues = await JiraClient.Issue.SearchAsync(issueSearch.Jql, fields: searchFields);

            var reportDataList = new List<ReportItem>();

            foreach (var issue in issues)
            {
                var worklog = await JiraClient.Work.GetAsync(issue.Key);
                if (worklog.Elements != null)
                {
                    var elements = worklog.Elements.Where(el =>
                        el.Updated >= getReportListParams.DateRange.Start 
                        && el.Updated <= getReportListParams.DateRange.End);

                    foreach (var element in elements)
                    {
                        var reportItem = new ReportItem
                        {
                            ProjectName = issue.Fields.Project.Name,
                            UserName = element.Author.DisplayName,
                            IssueKey = issue.Key,
                            IssueTitle = issue.Fields.Summary,
                            WorklogDescription = element.Comment
                        };

                        if (element.Updated != null)
                        {
                            reportItem.Date = element.Updated.Value.UtcDateTime;
                            reportDataList.Add(reportItem);
                        }

                        if (element.TimeSpentSeconds != null)
                        {
                            reportItem.Hours = (decimal)(element.TimeSpentSeconds.Value / 3600.0);
                        }

                        if (storyPointEstimateCustomField != null)
                        {
                            issue.Fields.CustomFields.TryGetValue(storyPointEstimateCustomField.Id, out var storyPointEstimate);
                            if (storyPointEstimate != null)
                            {
                                reportItem.StoryPointEstimate = Convert.ToDecimal(storyPointEstimate);
                            }
                        }
                    }
                }

                Console.WriteLine(worklog);
            }

            return reportDataList;
        }

        private async Task<List<ReportItem>> GetTempoReportDataList(GetReportListParams getReportListParams)
        {
            var worklogs = await TempoService.GetWorklogs(
                getReportListParams.DateRange.Start.Value, getReportListParams.DateRange.End.Value, getReportListParams.ProjectKey);

            var reportDataList = new List<ReportItem>();

            foreach (var worklog in worklogs.Items)
            {
                var reportItem = new ReportItem
                {
                    Date = worklog.WorkDate,
                    ProjectName = worklogs.ProjectKey,
                    UserName = worklog.Username,
                    IssueKey = worklog.IssueKey,
                    IssueTitle = worklog.IssueSummary,
                    WorklogDescription = worklog.WorkDescription,
                    Hours = worklog.Hours
                };
                reportDataList.Add(reportItem);
            }

            return reportDataList;
        }
    }
}
