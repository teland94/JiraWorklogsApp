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
        private ICipherService CipherService { get; }

        public JiraConnectionsService(AppDbContext context,
            ICipherService cipherService)
        {
            DbContext = context;
            CipherService = cipherService;
        }

        public async Task<int> CreateAsync(JiraConnection jiraConnection)
        {
            if (!string.IsNullOrEmpty(jiraConnection.AuthToken))
            {
                jiraConnection.AuthToken = CipherService.Encrypt(jiraConnection.AuthToken);
            }
            if (!string.IsNullOrEmpty(jiraConnection.TempoAuthToken))
            {
                jiraConnection.TempoAuthToken = CipherService.Encrypt(jiraConnection.TempoAuthToken);
            }

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

        public async Task<IEnumerable<JiraConnection>> GetAsync(string userId, bool decrypt = true)
        {
            IEnumerable<JiraConnection> jiraConnections;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                jiraConnections = await DbContext.JiraConnections.Where(c => c.UserId == userId).ToListAsync();
            }
            else
            {
                jiraConnections = await DbContext.JiraConnections.ToListAsync();
            }

            if (!decrypt) return jiraConnections;

            foreach (var jiraConnection in jiraConnections)
            {
                if (!string.IsNullOrEmpty(jiraConnection.AuthToken))
                {
                    jiraConnection.AuthToken = CipherService.Decrypt(jiraConnection.AuthToken);
                }
                if (!string.IsNullOrEmpty(jiraConnection.TempoAuthToken))
                {
                    jiraConnection.TempoAuthToken = CipherService.Decrypt(jiraConnection.TempoAuthToken);
                }
            }

            return jiraConnections;
        }

        public async Task<IEnumerable<JiraConnectionShortInfo>> GetShortInfoAsync()
        {
            return await DbContext.JiraConnections.Select(c => new JiraConnectionShortInfo
            {
                Id = c.Id,
                AuthToken = c.AuthToken
            }).ToListAsync();
        }

        public async Task<JiraConnection> GetAsync(int id, bool decrypt = true)
        {
            var jiraConnection = await DbContext.JiraConnections.FindAsync(id);

            if (!decrypt) return jiraConnection;

            if (!string.IsNullOrEmpty(jiraConnection.AuthToken))
            {
                jiraConnection.AuthToken = CipherService.Decrypt(jiraConnection.AuthToken);
            }
            if (!string.IsNullOrEmpty(jiraConnection.TempoAuthToken))
            {
                jiraConnection.TempoAuthToken = CipherService.Decrypt(jiraConnection.TempoAuthToken);
            }

            return jiraConnection;
        }

        public async Task UpdateAsync(JiraConnection jiraConnection)
        {
            var oldConnection = await DbContext.JiraConnections
                .Where(c => c.Id == jiraConnection.Id)
                .Select(c => new
                {
                    c.AuthToken,
                    c.TempoAuthToken
                }).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(jiraConnection.AuthToken) && oldConnection.AuthToken != jiraConnection.AuthToken)
            {
                jiraConnection.AuthToken = CipherService.Encrypt(jiraConnection.AuthToken);
            }
            if (!string.IsNullOrEmpty(jiraConnection.TempoAuthToken) && oldConnection.TempoAuthToken != jiraConnection.TempoAuthToken)
            {
                jiraConnection.TempoAuthToken = CipherService.Encrypt(jiraConnection.TempoAuthToken);
            }

            DbContext.JiraConnections.Update(jiraConnection);
            await DbContext.SaveChangesAsync();
        }

        public async Task TestAsync(JiraConnection jiraConnection)
        {
            var jiraClient = JiraClient.Create(new Uri(jiraConnection.InstanceUrl));
            jiraClient.SetTokenBasicAuthentication(jiraConnection.UserName, jiraConnection.AuthToken);
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
