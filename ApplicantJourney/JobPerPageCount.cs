using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    /// <summary>
    /// Represents a paged result set with metadata for any entity type
    /// </summary>
    /// <typeparam name="TEntityType">The type of entities in the paged result</typeparam>
    public class JobPerPageCount<TEntityType>
    {
        public IReadOnlyList<TEntityType> Items { get; set; } = new List<TEntityType>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public JobPerPageCount()
        {
        }

        public JobPerPageCount(IEnumerable<TEntityType> items, int totalCount, int page, int pageSize)
        {
            Items = items?.ToList() ?? new List<TEntityType>();
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}