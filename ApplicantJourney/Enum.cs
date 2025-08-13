using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    public enum ApplicationStatus
    {
        Applied,
        Interview,
        InProgress,
        Rejected,
        Offer
    }

    public enum UserJobApplicationSite
    {
        LinkedInEasyApply,
        CompanyWebsite,
        Email
    }

    public enum NotificationFrequency
    {
        Instant,
        NotificationFrequency
    }

    public enum JobType
    {
        FullTime,
        PartTime,
        Internship,
        Contract,
        Temporary
    }

    public enum JobListingSource   // website where the job is posted
    {// NOTE: avoid duplicate post, if same job is posted in multiple sites
        LinkedIn,
        Indeed,
        CompanyWebsite,
        Monster,
        Glassdoor,
        Other
    }
}
