using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApplicantJourney.JobListing;

namespace ApplicantJourney
{
    /*
      <summary>
     Represents a job listing scraped from job boards or company websites.
     </summary>
   */
    internal class JobListing
    {
        public int Id { get; set; }   // Unique identifier for each job
        public string JobTitle { get; set; }    //  e.g., Software Engineer
        public int Company { get; set; }  // ID of the company offering the job
        public DateTime JobPostingDate { get; set; }    // When the job was posted
        public DateTime JobExpirationDate { get; set; }   // When the job expires
        public string JobDescription { get; set; }
        public string ExperienceLevel { get; set; }
        /*
        public bool IsRemote { get; set; }
        public bool IsHybrid { get; set; }
        */
        public JobListingSource Source { get; set; }  // Source platform (e.g., LinkedIn, Indeed)
        public string Url { get; set; }
        public int ApplicantsCount { get; set; } // Estimated number of applicants
        public SalaryRange Salary { get; set; }            // Nested class for salary info
        public LocationInfo JobLocation { get; set; }         // Nested class for location info
        public JobType Type { get; set; }

        public void AddJobListing(   // Method to add a new job listing
            string title,
            int companyId,
            string description,
            JobType type,
            SalaryRange salary,
            LocationInfo location,
            JobListingSource source,
            string Joburl)
        {
            // Continue implementation
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

    public class SalaryRange // Nested class for salary information
    {
        public float Min { get; set; }               // Minimum salary
        public float Max { get; set; }               // Maximum salary
        public string Currency { get; set; }        // Currency (e.g., "USD", "EUR")
    }

    public class LocationInfo   // Nested class for location information
    {
        public string LocationType { get; set; }     // e.g., "Office", "Remote", "Hybrid"
        public bool IsRemote { get; set; }           // Fully remote position
        public bool IsHybrid { get; set; }           // Hybrid work acceptable
        public string PhysicalLocation { get; set; }  // Physical address/city if applicable
    }
}
