using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    /// <summary>
    /// data provider (e.g., Greenhouse, Workday).
    ///  fetch provider data and map it into domain JobListing objects.
    /// </summary>
    public interface IJobSource
    {
        Task<IReadOnlyList<JobListing>> FetchAsync(ProviderRequest request);
    }
}