using System.Text.Json.Serialization;

namespace JiraWorklogsApp.Common.Models
{
    public class PrivatBankExchangeInfoItem
    {
        [JsonPropertyName("ccy")]
        public string Ccy { get; set; }

        [JsonPropertyName("base_ccy")]
        public string BaseCcy { get; set; }

        [JsonPropertyName("buy")]
        public string Buy { get; set; }

        [JsonPropertyName("sale")]
        public string Sale { get; set; }
    }
}
