using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace ApplicantJourney
{
    /*
     <summary>
     Provides test data to simulate users, companies, job listings, applications, and resumes.
     </summary>
    */
    internal class TestData
    {
        public UserData CreateTestUser()
        {
            var userId = Guid.NewGuid();

            var user = new UserData
            {
                Id = userId,
                Name = "Kela Ise",
                Email = "kela@meta.com",
                Password = "TestPassword1", 

                Preferences = new JobPreferences
                {
                    PreferredJobTitles = new[] { "Seniour Software Engineer", "Seniour Backend Developer", "Junior Integrations Engineer" },
                    Locations = new[] { "Austin, TX", "Remote" },
                    IsRemote = true,
                    MinSalary = 85000f
                },

                Notifications = new NotificationSettings
                {
                    EmailAlerts = true,
                    PushNotifications = false,
                    Frequency = NotificationFrequency.Instant
                }
            };

            var company = CreateTestCompany();

            var job = new JobListing
            {
                JobId = 101,
                JobTitle = "Junior Software Engineer",
                CompanyId = company.Id,
                JobPostingDate = DateTime.Now.AddDays(-3),
                JobExpirationDate = DateTime.Now.AddDays(27),
                JobDescription = "Develop and maintain software solutions.",
                ExperienceLevel = "Entry Level",
                Source = JobListing.JobListingSource.CompanyWebsite,
                Url = "https://www.metacareers.com/jobs/683293827670564",
                ApplicantsCount = 42,
                Type = JobListing.JobType.FullTime,
                Salary = new SalaryRange
                {
                    Min = 180000f,
                    Max = 250000f,
                    Currency = "USD"
                },
                JobLocation = new LocationInfo
                {
                    LocationType = "Remote",
                    IsRemote = true,
                    IsHybrid = false,
                    PhysicalLocation = "N/A"
                }
            };

            user.SavedPositions.Add(job);

            var resume = new Resume
            {
                Id = 1,
                UserId = userId,
                FileUrl = "https://www.beamjobs.com/resumes/software-engineer-resume-examples",
                ResumeUploadTime = DateTime.Now.AddDays(-1),
                AtsScore = 78.5f,
                MissingKeywords = new List<string> { "CI/CD", "Docker" },
                FormatIssues = new List<string> { "Use of tables", "Font size inconsistency" },
                JobMatches = new List<JobMatch>
                {
                    new JobMatch
                    {
                        JobId = job.JobId,
                        MatchPercentage = 80.0f,
                        SuggestedImprovements = "Add CI/CD experience",
                        JobTitle = job.JobTitle,
                        JobDescription = job.JobDescription
                    }
                }
            };

            user.Resumes.Add(resume);

            var application = new ApplicationTracker
            {
                Id = 5001,
                UserId = 101,
                JobId = job.JobId,
                ApplicationTime = DateTime.Now.AddDays(-2),
                Status = ApplicationStatus.Applied,
                ResumeUsed = resume.FileUrl,
                AppicantNotes = "Excited about this opportunity. Matches my goals.",
                LastFollowUpTime = DateTime.Now.AddDays(-1),
                NextFollowUpTime = DateTime.Now.AddDays(3),
                SiteWhereUserApplied = UserJobApplicationSite.LinkedInEasyApply
            };

            user.TrackedApplications.Add(application);

            return user;
        }

        public CompanyData CreateTestCompany()
        {
            return new CompanyData
            {
                Id = 2001,
                Name = "Meta Inc.",
                HiringTrends = "Hiring aggressively in AI and backend roles.",
                AverageSalary = 92000f,
                JobSource = "CompanyWebsite",
                ReputationScore = 4.3f,
                EstimatedApplications = 150,
                CompetitionInsights = "High competition for remote roles, especially junior positions."
            };
        }
    }
}
