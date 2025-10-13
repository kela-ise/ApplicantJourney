using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    /// <summary>
    /// logic for fetching, filtering, sorting job listing page.
    /// </summary>
    public sealed class Logic
    {
        private readonly IJobSource _source;
        private readonly JobSearchService _service;

        public Logic(IJobSource source = null, JobSearchService service = null)
        {
            _source = source ?? new GreenhouseSource();
            _service = service ?? new JobSearchService();
        }

        public Task<IReadOnlyList<JobListing>> FetchAsync(string boardToken, int companyId, bool includeContent)
        {
            var request = new ProviderRequest
            {
                BoardToken = boardToken ?? string.Empty,
                CompanyId = companyId,
                IncludeContent = includeContent
            };
            return _service.FetchAndCacheAsync(_source, request);
        }

        public IEnumerable<JobListing> ApplyFilters(
            IEnumerable<JobListing> jobs,
            string titleContains = null,
            string locationContains = null,
            bool? remoteOnly = null,
            JobType? type = null,
            JobListingSource? source = null,
            DateTime? postedAfter = null)
            => _service.Filter(jobs, titleContains, locationContains, remoteOnly, type, source, postedAfter);

        public IEnumerable<JobListing> Dedupe(IEnumerable<JobListing> jobs) => _service.Dedupe(jobs);
        public IEnumerable<JobListing> Sort(IEnumerable<JobListing> jobs, string by, bool desc) => _service.Sort(jobs, by, desc);

        /// Returns the current page (1-based) of results, using validated Constants.DefaultPage/Size.
        public IEnumerable<JobListing> NumberOfPages(IEnumerable<JobListing> jobs, int page, int pageSize)
            => _service.NumberOfPages(jobs, page, pageSize);

        public async Task<IEnumerable<JobListing>> RunPipelineAsync(
            string boardToken,
            int companyId,
            bool includeContent,
            string titleContains = null,
            string locationContains = null,
            bool? remoteOnly = null,
            JobType? type = null,
            JobListingSource? source = null,
            DateTime? postedAfter = null,
            string sortBy = "date",
            bool sortDesc = true,
            int page = Constants.DefaultPage,
            int pageSize = Constants.DefaultPageSize)
        {
            var jobs = await FetchAsync(boardToken, companyId, includeContent).ConfigureAwait(false);
            var filtered = ApplyFilters(jobs, titleContains, locationContains, remoteOnly, type, source, postedAfter);
            var deduped = Dedupe(filtered);
            var sorted = Sort(deduped, sortBy, sortDesc);
            return NumberOfPages(sorted, page, pageSize);
        }
    }
}
