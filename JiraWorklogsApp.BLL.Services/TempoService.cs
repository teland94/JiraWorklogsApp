using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Models.Tempo;

namespace JiraWorklogsApp.BLL.Services
{
    public class TempoService : ITempoService
    {
        private readonly NameValueCollection _query;
        private string _baseUrl;
        private string _tempoApiToken;

        private TempoService(string baseUrl = null)
        {
            _query = HttpUtility.ParseQueryString(string.Empty);
            SetBaseUrl(baseUrl);
        }

        public static ITempoService Create(string baseUrl = null)
        {
            return new TempoService(baseUrl);
        }

        public void SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            _query["baseUrl"] = _baseUrl;
        }

        public void SetTempoApiToken(string tempoApiToken)
        {
            _tempoApiToken = tempoApiToken;
            _query["tempoApiToken"] = _tempoApiToken;
        }

        public async Task<Worklogs> GetWorklogs(DateTime dateFrom, DateTime dateTo, string projectKey)
        {
            _query[nameof(dateFrom)] = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            _query[nameof(dateTo)] = dateTo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            _query[nameof(projectKey)] = projectKey;
            _query["addIssueSummary"] = "true";
            _query["diffOnly"] = "true";
            _query["format"] = "xml";

            var result = await GetHttpResult("https://app.tempo.io/api/1/getWorklog");

            return XmlDeserialize<Worklogs>(result);
        }

        private async Task<string> GetHttpResult(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var queryString = _query.ToString();
                var builder = new UriBuilder(url) { Query = queryString };

                var response = await httpClient.GetAsync(builder.Uri);

                ResetQueryParams();

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        private void ResetQueryParams()
        {
            _query.Clear();
            _query["baseUrl"] = _baseUrl;
            _query["tempoApiToken"] = _tempoApiToken;
        }

        private static T XmlDeserialize<T>(string data)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(data))
            {
                return (T)serializer.Deserialize(sr);
            }
        }
    }
}
