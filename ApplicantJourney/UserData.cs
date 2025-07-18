using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /*
     <summary>
     Represents a user profile and their preferences
      <summary>
    */
    internal class UserData
    {

        public Guid UserId { get; set; }  // Unique identifier for the user
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Resume> Resumes { get; set; } // Uploaded resume


        public List<JobListing> SavedPositions { get; set; } // Jobs saved by the user
        public List<ApplicationTracker> TrackedApplications { get; set; }  // Applications tracked by the user

        public JobPreferences Preferences { get; set; }    // User’s job preferences


        public NotificationSettings Notifications { get; set; }  // Notification preferences


        public UserData()     // Constructor to initialize lists
        {
            Resumes = new List<Resume>();
            SavedPositions = new List<JobListing>();
            TrackedApplications = new List<ApplicationTracker>();
        }
    }

    public class JobPreferences
    {
        public string[] PreferredJobTitles { get; set; }
        public string[] Locations { get; set; }
        public bool IsRemote { get; set; }
        public float MinSalary { get; set; }
    }

    public class NotificationSettings
    {
        public bool EmailAlerts { get; set; }
        public bool PushNotifications { get; set; }
        public NotificationFrequency Frequency { get; set; }
    }

    public enum NotificationFrequency
    {
        Instant,
        NotificationFrequency

    }
}
