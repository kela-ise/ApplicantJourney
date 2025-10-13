using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// Implementation of IJobSource that pulls data from the Greenhouse API.
    /// Uses GreenhouseApiClient and converts API JSON to domain JobListing objects.
    /// </summary>
    public sealed class GreenhouseSource : IJobSource
    {
        private readonly GreenhouseApiClient _client = new GreenhouseApiClient();

        public async Task<IReadOnlyList<JobListing>> FetchAsync(ProviderRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var root = await _client.FetchJobsAsync(request.BoardToken, includeContent: request.IncludeContent)
                                    .ConfigureAwait(false);

            if (root?.Jobs == null || root.Jobs.Count == 0)
                return Array.Empty<JobListing>();

            return root.Jobs
                       .Select(j => j.ToJobListing(request.CompanyId))
                       .ToList();
        }
    }
}

