using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Models;

namespace JiraWorklogsApp.BLL.Services
{
    public class ExchangeInfoService : IExchangeInfoService
    {
        public async Task<IEnumerable<PrivatBankExchangeInfoItem>> GetPrivatBankExchangeInfoAsync()
        {
            var json = await SendGetRequestAsync("https://api.privatbank.ua/p24api/pubinfo?exchange&json&coursid=11");

            Debug.WriteLine(PrettyJson(json));

            return JsonSerializer.Deserialize<IEnumerable<PrivatBankExchangeInfoItem>>(json);
        }

        public async Task<PrivatBankExchangeInfoItem> GetPrivatBankExchangeInfoByCcyAsync(string ccy)
        {
            var privatBankExchangeInfoItems = await GetPrivatBankExchangeInfoAsync();
            var usdExchangeInfo = privatBankExchangeInfoItems.FirstOrDefault(e => e.Ccy == ccy);

            return usdExchangeInfo;
        }

        private static async Task<string> SendGetRequestAsync(string url)
        {
            using var http = new HttpClient();
            var res = await http.GetAsync(url);

            return await res.Content.ReadAsStringAsync();
        }

        private static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }
    }
}
