using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    public enum JobListingSource
    {// NOTE: avoid duplicate post, if same job is posted in multiple sites
        LinkedIn,
        Indeed,
        CompanyWebsite,
        Monster,
        Glassdoor,
        Other
    }
}
