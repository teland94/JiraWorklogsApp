using System.Collections.Generic;
using System.Threading.Tasks;
using JiraWorklogsApp.Common.Models;
using JiraWorklogsApp.DAL.Entities.Models;

namespace JiraWorklogsApp.BLL.IServices
{
    public interface IJiraConnectionsService
    {
        Task<IEnumerable<JiraConnection>> GetAsync(string userId);
        Task<IEnumerable<JiraConnectionShortInfo>> GetShortInfoAsync();
        Task<JiraConnection> GetAsync(int id);
        Task<int> CreateAsync(JiraConnection jiraConnection);
        Task UpdateAsync(JiraConnection jiraConnection);
        Task DeleteAsync(int id);

        Task TestAsync(JiraConnection jiraConnection);
    }
}
