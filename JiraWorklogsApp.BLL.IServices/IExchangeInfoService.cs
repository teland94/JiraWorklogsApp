using System.Collections.Generic;
using System.Threading.Tasks;
using JiraWorklogsApp.Common.Models;

namespace JiraWorklogsApp.BLL.IServices
{
    public interface IExchangeInfoService
    {
        Task<IEnumerable<PrivatBankExchangeInfoItem>> GetPrivatBankExchangeInfoAsync();

        Task<PrivatBankExchangeInfoItem> GetPrivatBankExchangeInfoByCcyAsync(string ccy);
    }
}