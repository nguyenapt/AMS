using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Detectify
{
    public class Detectify
    {
        private DetectifyConfiguration ClientConfiguration { get; }

        public Detectify()
            : this(new DetectifyConfiguration())
        { }
        protected Detectify(DetectifyConfiguration configuration)
        {
            ClientConfiguration = configuration;
        }


        /// <summary>
        /// Create the headers used to sign an API request.
        /// </summary>
        /// <param name="method">The method used for the call, in uppercase.</param>
        /// <param name="path">The path of the request, ie `/profiles/`.</param>
        /// <param name="timestamp">The timestamp used when creating the signature.</param>
        /// <param name="body">The body used for requests that require a provided payload. Must be null or an empty string if the request has no body.</param>
        /// <returns>Returns a dictionary of signature headers to use with an API call.</returns>
        private Dictionary<string, string> MakeHeaders(string method, string path,
            DateTime timestamp, string body)
        {
            // Signature timestamp is in Unix epoch format
            var epoch = (long) (timestamp - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;

            // Calculate signature hash
            var signatureValue = $"{method};{path};{ClientConfiguration.ApiKey};{epoch};{body}";
            var signatureBytes = new HMACSHA256(Convert.FromBase64String(ClientConfiguration.SecretKey))
                .ComputeHash(Encoding.Default.GetBytes(signatureValue));
            var signature = Convert.ToBase64String(signatureBytes);

            return new Dictionary<string, string>
            {
                ["X-Detectify-Key"] = ClientConfiguration.ApiKey,
                ["X-Detectify-Signature"] = signature,
                ["X-Detectify-Timestamp"] = epoch.ToString()
            };
        }

        public async Task<string> GetScanProfilesAsync()
        {
            var path = $"profiles/";
            var url = $"{ClientConfiguration.BaseAddress}{path}";
            var timestamp = DateTime.UtcNow;

            // Create Detectify headers
            var headers = MakeHeaders("GET", path, timestamp, null);

            using (var client = new HttpClient())
            {
                // Add Detectify headers to request
                headers.ToList().ForEach(h => client.DefaultRequestHeaders.Add(h.Key, h.Value));

                var response = await client.GetAsync(url);
                switch ((int)response.StatusCode)
                {
                    case 200:
                        return await response.Content.ReadAsStringAsync();
                    case 401:
                        throw new HttpRequestException("Missing/invalid API key or message signature, or invalid timestamp.");
                    case 403:
                        throw new HttpRequestException("The API key cannot access this functionality.");
                    case 502:
                        throw new HttpRequestException("Bad Gateway - The REST API is currently offline, possibly due to an upgrade. Please try again later.");
                    case 503:
                        throw new HttpRequestException("Service Unavailable - Temporary outage within the Detectify infrastructure, possibly due to an upgrade of a Detectify component. Please try again later.");
                    case 504:
                        throw new HttpRequestException("Gateway Timeout - Indicates that the request could not be processed in time, possibly due to overload. Please try again later.");
                    default:
                        throw new HttpRequestException($"API returned unhandled status code: {(int)response.StatusCode}");
                }
            }
        }

        /// <summary>
        /// Start a scan for a given scan profile.
        /// </summary>
        /// <param name="scanProfileToken">The scan profile to start a scan on.</param>
        /// <returns>Returns true if a scan was started, false if not.</returns>
        public async Task<bool> StartScanAsync(string scanProfileToken)
        {
            var path = $"scans/{scanProfileToken}/";
            var url = $"{ClientConfiguration.BaseAddress}{path}";
            var timestamp = DateTime.UtcNow;

            // Create Detectify headers
            var headers = MakeHeaders("POST", path, timestamp, null);

            using (var client = new HttpClient())
            {
                // Add Detectify headers to request
                headers.ToList().ForEach(h => client.DefaultRequestHeaders.Add(h.Key, h.Value));

                var response = await client.PostAsync(url, null);

                switch ((int) response.StatusCode)
                {
                    case 202:
                        Console.WriteLine("Scan start request accepted");
                        return true;
                    case 400:
                        Console.WriteLine("Invalid scan profile token");
                        return false;
                    case 401:
                        Console.WriteLine("Missing/invalid API key or message signature, or invalid timestamp");
                        return false;
                    case 403:
                        Console.WriteLine("The API key cannot access this functionality");
                        return false;
                    case 404:
                        Console.WriteLine(
                            "The specified scan profile does not exist or the API cannot access the profile");
                        return false;
                    case 409:
                        Console.WriteLine("A scan is already running on the specified profile");
                        return false;
                    case 423:
                        Console.WriteLine("The domain is not verified");
                        return false;
                    case 500:
                    case 503:
                        Console.WriteLine("An error occurred while processing the request");
                        return false;
                    default:
                        Console.WriteLine($"API returned unhandled status code: {(int) response.StatusCode}");
                        return false;
                }
            }
        }

        /// <summary>
        /// Retrieves the status of a currently running scan for a given scan profile.
        /// </summary>
        /// <param name="scanProfileToken">The scan profile token to check scan status for.</param>
        public async Task ScanStatusAsync(string scanProfileToken)
        {
            var path = $"scans/{scanProfileToken}/";
            var url = $"{ClientConfiguration.BaseAddress}{path}";
            var timestamp = DateTime.UtcNow;
            
            // Create Detectify headers
            var headers = MakeHeaders("GET", path, timestamp, null);

            using (var client = new HttpClient())
            {
                // Add Detectify headers to request
                headers.ToList().ForEach(h => client.DefaultRequestHeaders.Add(h.Key, h.Value));

                var response = await client.GetAsync(url);

                switch ((int) response.StatusCode)
                {
                    case 200:
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                        break;
                    case 400:
                        Console.WriteLine("Invalid scan profile token");
                        break;
                    case 401:
                        Console.WriteLine("Missing/invalid API key or message signature, or invalid timestamp");
                        break;
                    case 403:
                        Console.WriteLine("The API key cannot access this functionality");
                        break;
                    case 404:
                        Console.WriteLine(
                            "No scan running for the specified profile, or the specified scan profile does not exist, or the API cannot access the profile");
                        break;
                    case 500:
                    case 503:
                        Console.WriteLine("An error occurred while processing the request");
                        break;
                    default:
                        Console.WriteLine($"API returned unhandled status code: {(int) response.StatusCode}");
                        break;
                }
            }
        }
    }
}
