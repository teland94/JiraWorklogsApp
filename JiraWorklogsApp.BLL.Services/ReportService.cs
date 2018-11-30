using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapplo.Jira;
using Dapplo.Jira.Entities;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Helpers;
using JiraWorklogsApp.Common.Models;

namespace JiraWorklogsApp.BLL.Services
{
    public class ReportService : IReportService
    {
        private IJiraClient JiraClient { get; }
        private IJiraConnectionsService JiraConnectionService { get; }
        private ITempoService TempoService { get; }

        public ReportService(IJiraConnectionsService jiraConnectionService)
        {
            JiraClient = Dapplo.Jira.JiraClient.Create();
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
                JiraClient.SetBaseUri(new Uri(connection.InstanceUrl));
                JiraClient.SetTokenBasicAuthentication(connection.UserName, token);

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

        public async Task<IEnumerable<ReportItem>> GetReportListAsync(GetReportListParams getReportListParams)
        {
            return await GetReportDataListAsync(getReportListParams);
        }

        public async Task<byte[]> GetReportExcelDataAsync(GetReportListParams getReportListParams)
        {
            var data = await GetReportDataListAsync(getReportListParams);
            return ExcelHelper.GetExportFile(data, "Report");
        }

        private async Task<List<ReportItem>> GetReportDataListAsync(GetReportListParams getReportListParams)
        {
            var connection = await JiraConnectionService.GetAsync(getReportListParams.JiraConnection.Id);

            List<ReportItem> reportDataList;

            if (string.IsNullOrWhiteSpace(connection.TempoAuthToken) &&
                string.IsNullOrWhiteSpace(getReportListParams.JiraConnection.TempoAuthToken))
            {
                var token = string.IsNullOrWhiteSpace(connection.AuthToken) ? getReportListParams.JiraConnection.AuthToken : connection.AuthToken;
                JiraClient.SetBaseUri(new Uri(connection.InstanceUrl));
                JiraClient.SetTokenBasicAuthentication(connection.UserName, token);
                reportDataList = await GetJiraReportDataList(getReportListParams);
            }
            else
            {
                var token = string.IsNullOrWhiteSpace(connection.TempoAuthToken) ? getReportListParams.JiraConnection.TempoAuthToken : connection.TempoAuthToken;
                TempoService.SetBaseUrl(connection.InstanceUrl);
                TempoService.SetTempoApiToken(token);

                reportDataList = await GetTempoReportDataList(getReportListParams);
            }

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

            var issueSearch = new JqlIssueSearch {Jql = $"project='{getReportListParams.ProjectKey}'" +
                                                        $"AND worklogDate >= {startDate} AND worklogDate <= {endDate}"};

            var issues = await JiraClient.Issue.SearchAsync(issueSearch, null);

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
