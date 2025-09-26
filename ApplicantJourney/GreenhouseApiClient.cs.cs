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
    /// Fetches job listings for a given company board token.
    /// </summary>
    public class GreenhouseApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Fetch jobs for the given Greenhouse board token.
        /// Example token: "vaulttec"
        /// API: https://boards-api.greenhouse.io/v1/boards/{board_token}/jobs
        /// </summary>
        public async Task<GreenhouseRoot?> FetchJobsAsync(string boardToken)
        {
            try
            {
                string url = $"https://boards-api.greenhouse.io/v1/boards/{boardToken}/jobs";
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<GreenhouseRoot>(responseBody, options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message}");
                return null;
            }
        }
    }
}

