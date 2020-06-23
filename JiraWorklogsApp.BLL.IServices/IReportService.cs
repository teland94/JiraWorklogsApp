using System.Collections.Generic;
using System.Threading.Tasks;
using JiraWorklogsApp.Common.Models;
using JiraWorklogsApp.Common.Models.Params;

namespace JiraWorklogsApp.BLL.IServices
{
    public interface IReportService
    {
        Task<IEnumerable<JiraProject>> GetProjectsAsync(ICollection<JiraConnectionShortInfo> jiraConnections, string userId);

        Task<IEnumerable<JiraUser>> GetAssignableUsers(GetAssignableUsersParams getAssignableUsersParams);

        Task<IEnumerable<ReportItem>> GetReportListAsync(GetReportListParams getReportListParams);

        Task<byte[]> GetReportExcelDataAsync(GetReportListParams getReportListParams);
    }
}