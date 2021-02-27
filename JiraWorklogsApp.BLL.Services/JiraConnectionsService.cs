using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapplo.Jira;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Models;
using JiraWorklogsApp.DAL.Entities.Models;
using JiraWorklogsApp.DAL.Persistance;
using Microsoft.EntityFrameworkCore;

namespace JiraWorklogsApp.BLL.Services
{
    public class JiraConnectionsService : IJiraConnectionsService
    {
        private AppDbContext DbContext { get; }

        public JiraConnectionsService(AppDbContext context)
        {
            DbContext = context;
        }

        public async Task<int> CreateAsync(JiraConnection jiraConnection)
        {
            var entityEntry = await DbContext.JiraConnections.AddAsync(jiraConnection);
            await DbContext.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var connection = await DbContext.JiraConnections.FindAsync(id);
            DbContext.JiraConnections.Remove(connection);
            await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<JiraConnection>> GetAsync(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                return await DbContext.JiraConnections.Where(c => c.UserId == userId).ToListAsync();
            }
            else
            {
                return await DbContext.JiraConnections.ToListAsync();
            }
        }

        public async Task<IEnumerable<JiraConnectionShortInfo>> GetShortInfoAsync()
        {
            return await DbContext.JiraConnections.Select(c => new JiraConnectionShortInfo
            {
                Id = c.Id,
                AuthToken = c.AuthToken
            }).ToListAsync();
        }

        public async Task<JiraConnection> GetAsync(int id)
        {
            return await DbContext.JiraConnections.FindAsync(id);
        }

        public async Task UpdateAsync(JiraConnection jiraConnection)
        {
            DbContext.JiraConnections.Update(jiraConnection);
            await DbContext.SaveChangesAsync();
        }

        public async Task TestAsync(JiraConnection jiraConnection)
        {
            var jiraClient = JiraClient.Create(new Uri(jiraConnection.InstanceUrl));
            jiraClient.SetBasicAuthentication(jiraConnection.UserName, jiraConnection.AuthToken);
            await jiraClient.User.GetMyselfAsync();

            if (!string.IsNullOrWhiteSpace(jiraConnection.TempoAuthToken))
            {
                var tempoService = TempoService.Create(jiraConnection.InstanceUrl);
                tempoService.SetTempoApiToken(jiraConnection.TempoAuthToken);
                await tempoService.GetWorklogs(DateTime.Now, DateTime.Now, "XXX");
            }
        }
    }
}
