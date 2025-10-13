using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// Encapsulates parameters for requesting jobs from a provider.
    /// </summary>
    public sealed class ProviderRequest
    {
        
        public string BoardToken { get; init; } = string.Empty; //Provider-specific board token (e.g., Greenhouse board key).

        public int CompanyId { get; init; } = 0;  // Optional internal company ID used for mapping.

        public bool IncludeContent { get; init; } = true;   // Whether to include full HTML job content when supported.

    }
}
