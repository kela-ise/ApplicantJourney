using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// logic for filtering and sorting job listings.
    /// </summary>
    public sealed class JobSearchService
    {
        private IReadOnlyList<JobListing> _cache = Array.Empty<JobListing>();

        public async Task<IReadOnlyList<JobListing>> FetchAndCacheAsync(IJobSource source, ProviderRequest request)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (request == null) throw new ArgumentNullException(nameof(request));

            _cache = await source.FetchAsync(request).ConfigureAwait(false);
            return _cache;
        }

        public IEnumerable<JobListing> Filter(
            IEnumerable<JobListing> jobs,
            string? titleContains = null,
            string? locationContains = null,
            bool? remoteOnly = null,
            JobType? type = null,
            JobListingSource? source = null,
            DateTime? postedAfter = null)
        {
            if (jobs == null) return Enumerable.Empty<JobListing>();

            var q = jobs;

            if (!string.IsNullOrWhiteSpace(titleContains))
                q = q.Where(j => (j.JobTitle ?? string.Empty).Contains(titleContains, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(locationContains))
                q = q.Where(j => (j.JobLocation?.PhysicalLocation ?? string.Empty)
                    .Contains(locationContains, StringComparison.OrdinalIgnoreCase));

            if (remoteOnly == true)
                q = q.Where(j => j.JobLocation?.IsRemote == true);

            if (type.HasValue)
                q = q.Where(j => j.Type == type.Value);

            if (source.HasValue)
                q = q.Where(j => j.Source == source.Value);

            if (postedAfter.HasValue)
                q = q.Where(j => j.JobPostingDate >= postedAfter.Value);

            return q;
        }

        public IEnumerable<JobListing> Sort(IEnumerable<JobListing> jobs, string by, bool desc)
        {
            if (jobs == null) return Enumerable.Empty<JobListing>();

            return (by?.ToLowerInvariant(), desc) switch
            {
                ("title", false) => jobs.OrderBy(j => j.JobTitle),
                ("title", true) => jobs.OrderByDescending(j => j.JobTitle),
                ("company", false) => jobs.OrderBy(j => j.Company),
                ("company", true) => jobs.OrderByDescending(j => j.Company),
                ("date", false) => jobs.OrderBy(j => j.JobPostingDate),
                _ => jobs.OrderByDescending(j => j.JobPostingDate)
            };
        }

        /// <summary>
        /// Returns the jobs for a given page number and size.
        /// Default page = 1, size = 100 (Constants).
        /// </summary>
        public IEnumerable<JobListing> NumberOfPages(IEnumerable<JobListing> jobs, int page, int pageSize)
        {
            if (jobs == null) return Enumerable.Empty<JobListing>();
            if (page < Constants.DefaultPage) page = Constants.DefaultPage;
            if (pageSize < Constants.DefaultPageSize) pageSize = Constants.DefaultPageSize;

            return jobs.Skip((page - Constants.DefaultPage) * pageSize)
                       .Take(pageSize);
        }

        /// <summary>
        /// Deduplicate jobs by URL if present; otherwise by (Title + Location).
        /// Iterator method uses 'yield return'; for null input we 'yield break' to satisfy C# rules.
        /// </summary>
        public IEnumerable<JobListing> Dedupe(IEnumerable<JobListing> jobs)
        {
            if (jobs == null) yield break;

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var j in jobs)
            {
                var key = !string.IsNullOrWhiteSpace(j.Url)
                    ? $"url:{j.Url}"
                    : $"titleloc:{j.JobTitle}|{j.JobLocation?.PhysicalLocation}";
                if (seen.Add(key))
                    yield return j;
            }
        }
    }
}
