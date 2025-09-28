using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    /// <summary>
    /// Client for interacting with the Greenhouse API.
    /// </summary>
    public class GreenhouseApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Fetch jobs for the given Greenhouse board token.
        /// Set includeContent=true to retrieve job descriptions.
        /// </summary>
        public async Task<GreenhouseRoot?> FetchJobsAsync(string boardToken, bool includeContent = true)
        {
            var url = $"https://boards-api.greenhouse.io/v1/boards/{boardToken}/jobs" +
                      (includeContent ? "?content=true" : string.Empty);

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<GreenhouseRoot>(json, options);
        }
    }
}

