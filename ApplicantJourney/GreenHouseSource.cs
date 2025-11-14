using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// Implementation of IJobSource that pulls data from the Greenhouse API.
    /// Uses GreenhouseApiClient and converts API JSON to domain JobListing objects.
    /// Updated to support multiple companies via board token mapping.
    /// </summary>
    public sealed class GreenhouseSource : IJobSource
    {
        private readonly GreenhouseApiClient _client = new GreenhouseApiClient();

        public async Task<IReadOnlyList<JobListing>> FetchAsync(ProviderRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // If no specific board token provided, fetch from all configured companies
            if (string.IsNullOrWhiteSpace(request.BoardToken))
            {
                return await FetchFromAllCompanies(request.IncludeContent);
            }

            // Fetch from specific board token
            var root = await _client.FetchJobsAsync(request.BoardToken, includeContent: request.IncludeContent)
                                    .ConfigureAwait(false);

            if (root?.Jobs == null || root.Jobs.Count == 0)
                return Array.Empty<JobListing>();

            // Map company ID if we can identify the company from the board token
            var companyData = CompanyConfiguration.GetCompanyByToken(request.BoardToken);
            var companyId = companyData?.Id ?? request.CompanyId;

            return root.Jobs
                       .Select(j => j.ToJobListing(companyId))
                       .ToList();
        }

        private async Task<IReadOnlyList<JobListing>> FetchFromAllCompanies(bool includeContent)
        {
            var allJobs = new List<JobListing>();

            foreach (var company in CompanyConfiguration.Companies)
            {
                try
                {
                    Console.WriteLine($"Fetching jobs for {company.Name} (Token: {company.BoardToken})");

                    var root = await _client.FetchJobsAsync(company.BoardToken, includeContent)
                                            .ConfigureAwait(false);

                    if (root?.Jobs != null && root.Jobs.Count > 0)
                    {
                        var companyJobs = root.Jobs.Select(j => j.ToJobListing(company.Id)).ToList();
                        allJobs.AddRange(companyJobs);
                        Console.WriteLine($"Retrieved {companyJobs.Count} jobs from {company.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching jobs from {company.Name}: {ex.Message}"); // Continue with other companies even if one fails
                }
            }

            return allJobs;
        }
    }
}