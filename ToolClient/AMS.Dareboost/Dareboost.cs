using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Dareboost
{
    public class Dareboost
    {
        /*Quotas:
        The Dareboost API is based on a credit system.
        Launching an analysis from the API costs you 1 credit.If you want additional metrics like visual metrics(first / last visual change, speedindex, video, etc), it costs you 1 additional credit.
        PDF generation costs you 2 credits per request.
        Others requests (failed analysis, get configuration, get a report, etc) don't cost anything.
        */

        private DareboostConfiguration ClientConfiguration { get; }

        public Dareboost()
            : this(new DareboostConfiguration())
        { }
        protected Dareboost(DareboostConfiguration configuration)
        {
            ClientConfiguration = configuration;
        }

        public class PostBodyConfig
        {
            //[Required]
            public string token { get; set; }

            public string location { get; set; }
        }

        /// <summary>
        /// Create the headers used to sign an API request.
        /// </summary>
        /// <returns>Returns a dictionary of headers to use with an API call.</returns>
        private Dictionary<string, string> MakeHeaders()
        {
            return new Dictionary<string, string>
            {
                ["token"] = ClientConfiguration.ApiToken,
                ["Content-Type"] = "application/json"
            };
        }

        public async Task<string> GetConfigAsync()
        {
            var path = $"config";
            var url = $"{ClientConfiguration.BaseAddress}{path}";

            // Create request headers
            //var headers = MakeHeaders();

            using (var client = new HttpClient())
            {
                // Add headers to request
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //headers.ToList().ForEach(h => client.DefaultRequestHeaders.Add(h.Key, h.Value));
                //var response = await client.PostAsync(url, new StringContent("{'token': '" + ClientConfiguration.ApiToken + "'}", Encoding.UTF8, "application/json"));

                var bodyMsg = new PostBodyConfig() { token = ClientConfiguration.ApiToken };
                var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(bodyMsg), Encoding.UTF8, "application/json"));
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
        }
    }
}
