using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 <summary>
 Provides test data to simulate users, companies, job listings, applications, and resumes.
 Also supports saving/loading the generated test data using XML serialization.
 </summary>
*/
namespace ApplicantJourney
{
    public class TestData
    {
        private static readonly string FilePath = Constants.TestDataFileName;

        public UserData CreateTestUser()
        {
            int userId = Constants.DefaultUserId;

            var user = new UserData
            {
                Id = userId,
                Name = "Kela Best",
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

            var company = TestData.CreateTestCompany();

            var job = new JobListing
            {
                Id = 101,
                JobTitle = "Junior Software Engineer",
                Company = company.Id,
                JobPostingDate = DateTime.Now.AddDays(-3),
                JobExpirationDate = DateTime.Now.AddDays(27),
                JobDescription = "Develop and maintain software solutions.",
                ExperienceLevel = "Entry Level",
                Source = JobListingSource.CompanyWebsite,
                Url = "https://www.metacareers.com/jobs/683293827670564",
                ApplicantsCount = 42,
                Type = JobType.FullTime,
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
                User = userId,
                FileUrl = "https://www.beamjobs.com/resumes/software-engineer-resume-examples",
                ResumeUploadTime = DateTime.Now.AddDays(-1),
                AtsScore = 78.5f,
                MissingKeywords = new List<string> { "CI/CD", "Docker" },
                FormatIssues = new List<string> { "Use of tables", "Font size inconsistency" },
                JobMatches = new List<JobMatch>
                {
                    new JobMatch
                    {
                        Id = job.Id,
                        MatchPercentage = 80.0f,
                        SuggestedImprovements = "Add CI/CD experience",
                        JobTitle = job.JobTitle,
                        JobDescription = job.JobDescription
                    }
                }
            };

            user.Resumes.Add(resume);

            var application = new Application
            {
                Id = 5001,
                ApplicantNumber = 101,
                Position = job.Id,
                ApplicationTime = DateTime.Now.AddDays(-2),
                Status = ApplicationStatus.Applied,
                ResumeUsed = resume.FileUrl,
                AppicantNotes = "Excited about this opportunity. Matches my goals.",
                LastFollowUpTime = DateTime.Now.AddDays(-1),
                NextFollowUpTime = DateTime.Now.AddDays(3),
                SiteWhereUserApplied = UserJobApplicationSite.LinkedInEasyApply
            };

            user.TrackedApplications.Add(application);

            // Save the generated user data to file
            SaveTestUser(user);

            return user;
        }

        /// <summary>
        /// Creates a sample company for test data.
        /// Now marked static because it does not rely on instance members.
        /// </summary>
        public static CompanyData CreateTestCompany()
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

        /// <summary>
        /// Save user test data to XML file (safe write with descriptive temp file).
        /// Made public so callers can persist changes any time.
        /// </summary>
        public void SaveTestUser(UserData user)
        {
            var serializer = new XmlSerializer(typeof(UserData));

            // Use a descriptive name for the temp file (e.g., "TestData.tempSerializationFile.xml")
            var tempFilePath = FilePath.Replace(".xml", Constants.TempSerializationSuffix);

            using (var writer = new StreamWriter(tempFilePath))
            {
                serializer.Serialize(writer, user);
            }

            File.Copy(tempFilePath, FilePath, overwrite: true); // overwrite safely
            File.Delete(tempFilePath);
        }

        /// <summary>
        /// Load user test data from XML file (short version).
        /// Returns null if the file does not exist.
        /// </summary>
        public UserData? LoadTestUser()
        {
            if (!File.Exists(FilePath))
            {
                return null; // No saved file found
            }

            var serializer = new XmlSerializer(typeof(UserData));
            using (var reader = new StreamReader(FilePath))
            {
                return serializer.Deserialize(reader) as UserData;
            }
        }
    }
}
