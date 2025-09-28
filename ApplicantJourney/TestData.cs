using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ApplicantJourney
{
    /*
     <summary>
     Provides test data to simulate users, companies, job listings, applications, and resumes.
     Also supports saving/loading the generated test data using XML serialization.
     </summary>
    */
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
                    PreferredJobTitles = new[] { "Senior Software Engineer", "Senior Backend Developer", "Junior Integrations Engineer" },
                    Locations = new[] { "Austin, TX", "Remote" },
                    IsRemote = true,
                    MinSalary = Constants.DefaultAverageSalarySample
                },

                Notifications = new NotificationSettings
                {
                    EmailAlerts = true,
                    PushNotifications = false,
                    Frequency = NotificationFrequency.Instant
                }
            };

            var company = TestData.CreateTestCompany();
            var postingDate = DateTime.Now.AddDays(-Constants.DefaultJobPostingDaysAgoSample);

            var job = new JobListing
            {
                Id = Constants.DefaultJobIdSample,
                JobTitle = "Junior Software Engineer",
                Company = company.Id,
                JobPostingDate = postingDate,
                JobExpirationDate = postingDate.AddDays(Constants.DefaultJobExpirationDays),

                // Sample uses plain sentence; store as raw HTML
                JobDescriptionHtml = "Develop and maintain software solutions.",

                ExperienceLevel = Constants.DefaultExperienceLevelEntry,
                Source = JobListingSource.CompanyWebsite,
                Url = "https://www.metacareers.com/jobs/683293827670564",
                ApplicantsCount = Constants.DefaultApplicantsCountSample,
                Type = JobType.FullTime,
                Salary = new SalaryRange
                {
                    Min = Constants.DefaultMinSalarySample,
                    Max = Constants.DefaultMaxSalarySample,
                    Currency = Constants.DefaultCurrency
                },
                JobLocation = new LocationInfo
                {
                    LocationType = Constants.LocationTypeRemote,
                    IsRemote = true,
                    IsHybrid = false,
                    PhysicalLocation = Constants.LocationNotAvailable
                }
            };

            user.SavedPositions.Add(job);

            var resume = new Resume
            {
                Id = Constants.DefaultResumeIdSample,
                User = userId,
                FileUrl = "https://www.beamjobs.com/resumes/software-engineer-resume-examples",
                ResumeUploadTime = DateTime.Now.AddDays(-Constants.LastFollowUpDaysAgoSample),
                AtsScore = Constants.DefaultAtsScoreSample,
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
                        JobDescription = "Develop and maintain software solutions."
                    }
                }
            };

            user.Resumes.Add(resume);

            var application = new Application
            {
                Id = Constants.DefaultApplicationIdSample,
                ApplicantNumber = Constants.DefaultApplicantNumberSample,
                Position = job.Id,
                ApplicationTime = DateTime.Now.AddDays(-Constants.ApplicationDaysAgoSample),
                Status = ApplicationStatus.Applied,
                ResumeUsed = resume.FileUrl,
                ApplicantNotes = "Excited about this opportunity. Matches my goals.",
                LastFollowUpTime = DateTime.Now.AddDays(-Constants.LastFollowUpDaysAgoSample),
                NextFollowUpTime = DateTime.Now.AddDays(Constants.NextFollowUpDaysFromNowSample),
                SiteWhereUserApplied = UserJobApplicationSite.LinkedInEasyApply
            };

            user.TrackedApplications.Add(application);

            // Save the generated user data to file
            SaveTestUser(user);

            return user;
        }

        public static CompanyData CreateTestCompany()
        {
            return new CompanyData
            {
                Id = Constants.DefaultCompanyIdSample,
                Name = "Meta Inc.",
                HiringTrends = "Hiring aggressively in AI and backend roles.",
                AverageSalary = Constants.DefaultAverageSalarySample,
                JobSource = JobListingSource.CompanyWebsite.ToString(),
                ReputationScore = 4.3f,
                EstimatedApplications = 150,
                CompetitionInsights = "High competition for remote roles, especially junior positions."
            };
        }

        public void SaveTestUser(UserData user)
        {
            var serializer = new XmlSerializer(typeof(UserData));
            var tempFilePath = FilePath.Replace(".xml", Constants.TempSerializationSuffix);

            using (var writer = new StreamWriter(tempFilePath))
            {
                serializer.Serialize(writer, user);
            }

            File.Copy(tempFilePath, FilePath, overwrite: true);
            File.Delete(tempFilePath);
        }

        public UserData? LoadTestUser()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(UserData));
            using (var reader = new StreamReader(FilePath))
            {
                return serializer.Deserialize(reader) as UserData;
            }
        }
    }
}
