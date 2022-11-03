using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMS.PageSpeed
{
    public class PageSpeedService
    {
        private PageSpeedConfiguration ClientConfiguration { get; }
        public PageSpeedService() : this(new PageSpeedConfiguration())
        { }

        public PageSpeedService(PageSpeedConfiguration configuration)
        {
            ClientConfiguration = configuration;
        }

        public async Task<string> PageSpeedRequest(HttpMethod method, string apiName, Dictionary<string, string> listParam)
        {
            var path = apiName;
            var url = $"{ClientConfiguration.BaseAddress}{path}";
            var apiKey = $"{ClientConfiguration.ApiKey}";

            var param = "";
            foreach (var item in listParam)
            {
                param += $"&{item.Key}={item.Value}";
            }
            var request = url + $"?captchaToken={apiKey}{param}&key={apiKey}";
            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = method;
            httpRequest.RequestUri = new Uri(request);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}", apiKey))));
                var response = await client.SendAsync(httpRequest);
                switch ((int)response.StatusCode)
                {
                    case 200:
                        return await response.Content.ReadAsStringAsync();
                    case 400:
                        throw new HttpRequestException("Missing parameters or bad value.");
                    case 401:
                        throw new HttpRequestException("Authentication required (no token or invalid token).");
                    case 403:
                        throw new HttpRequestException("Action forbidden (quotas reached, unauthorized url).");
                    case 404:
                        throw new HttpRequestException("Page is unreachable (unknown url / check server response failed).");
                    case 406:
                        throw new HttpRequestException("Not a valid json format.");
                    case 408:
                        throw new HttpRequestException("The analysis has timeout.");
                    case 417:
                        throw new HttpRequestException("The last analysis of the monitoring results in error.");
                    case 500:
                        throw new HttpRequestException("Internal server error.");
                    case 503:
                        throw new HttpRequestException("The API is temporarily unavailable, try again in a few minutes.");
                    default:
                        throw new HttpRequestException($"API returned unhandled status code: {(int)response.StatusCode}");
                }
            }


            return null;
        }
    }
}
