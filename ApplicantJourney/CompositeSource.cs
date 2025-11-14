using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// Aggregates multiple IJobSource providers and returns a merged list.
    /// Updated with debug logging and multi-company support.
    /// </summary>
    public sealed class CompositeSource : IJobSource
    {
        private readonly IReadOnlyList<IJobSource> _sources;

        public CompositeSource(IEnumerable<IJobSource> sources)
        {
            _sources = sources?.ToList() ?? throw new ArgumentNullException(nameof(sources));
            if (_sources.Count == 0)
                throw new ArgumentException("At least one source is required.", nameof(sources));
        }

        /// <summary>
        /// Fetch from all sources in parallel and merge results.
        /// NOTE: No sorting by PostedAt/PostedAtUtc here because the current JobListing
        /// model in your project does not expose those properties. We keep ordering stable.
        /// </summary>
        public async Task<IReadOnlyList<JobListing>> FetchAsync(ProviderRequest request)
        {
            Console.WriteLine($"CompositeSource: Fetching from {_sources.Count} sources in parallel");

            var tasks = _sources.Select(source => source.FetchAsync(request)).ToArray();
            await Task.WhenAll(tasks).ConfigureAwait(false);

            var combinedJobs = new List<JobListing>(tasks.Sum(task => task.Result.Count));
            foreach (var task in tasks)
                combinedJobs.AddRange(task.Result);

            Console.WriteLine($"CompositeSource: Combined {combinedJobs.Count} jobs from all sources");

            // If/when you reintroduce a date field on JobListing, you can sort here.
            // e.g., combined = combined.OrderByDescending(j => j.PostedAt ?? DateTime.MinValue).ToList();

            return combinedJobs;
        }
    }
}