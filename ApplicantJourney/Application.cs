using ApplicantJourney;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


public class Application
{
    public int Id { get; set; }                // Unique application ID
    public int ApplicantNumber { get; set; }
    public int Position { get; set; }
    public DateTime ApplicationTime { get; set; }
    public ApplicationStatus Status { get; set; }
    public string ResumeUsed { get; set; } = string.Empty; //  Initialize
    [XmlElement("AppicantNotes")] // keept so that XML serialization/deserialization(backward compatability) still works with previously saved data.
    public string ApplicantNotes { get; set; } = string.Empty; // Optional notes by the user
    public DateTime LastFollowUpTime { get; set; }   // Follow-up details
    public DateTime NextFollowUpTime { get; set; }
    public UserJobApplicationSite SiteWhereUserApplied { get; set; }   // Application platform used
}