using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    public class Application
    {
        public int Id { get; set; }                 // Unique application ID
        public int ApplicantNumber { get; set; }
        public int Position { get; set; }
        public DateTime ApplicationTime { get; set; }
        public ApplicationStatus Status { get; set; }
        public string ResumeUsed { get; set; }
        [XmlElement("AppicantNotes")] // keept so that XML serialization/deserialization still works with previously saved data.
        public string ApplicantNotes { get; set; }  // Optional notes by the user
        public DateTime LastFollowUpTime { get; set; } // Follow-up details
        public DateTime NextFollowUpTime { get; set; }
        public UserJobApplicationSite SiteWhereUserApplied { get; set; } // Application platform used
    }
}
