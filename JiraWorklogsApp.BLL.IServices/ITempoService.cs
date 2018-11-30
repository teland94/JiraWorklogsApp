using System;
using System.Threading.Tasks;
using JiraWorklogsApp.Common.Models.Tempo;

namespace JiraWorklogsApp.BLL.IServices
{
    public interface ITempoService
    {
        void SetBaseUrl(string baseUrl);

        void SetTempoApiToken(string tempoApiToken);

        Task<Worklogs> GetWorklogs(DateTime dateFrom, DateTime dateTo, string projectKey);
    }
}