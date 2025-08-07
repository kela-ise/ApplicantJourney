using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    internal class Application
    {

        public int Id { get; set; }    // Unique application ID
        public int User { get; set; }
        public int Position { get; set; }
        public DateTime ApplicationTime { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ResumeUsed { get; set; }
        public string AppicantNotes { get; set; } // Optional notes by the user
        public DateTime LastFollowUpTime { get; set; } // Follow-up details
        public DateTime NextFollowUpTime { get; set; }
        public UserJobApplicationSite SiteWhereUserApplied { get; set; } // Application platform used
    }
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
}
