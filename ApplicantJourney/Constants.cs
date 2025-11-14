using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicantJourney
{
    /// <summary>
    /// Centralized, reusable constants to avoid magic numbers/strings throughout the project
    /// </summary>
    public static class Constants
    {
        // Files & serialization
        public const string TestDataFileName = "TestData.xml";
        public const string TempSerializationSuffix = ".tempSerializationFile.xml";

        // Common indexes
        public const int FirstIndex = 0;

        // Defaults / IDs for sample data
        public const int DefaultUserId = 1001;
        public const int DefaultCompanyIdSample = 2001;
        public const int DefaultJobIdSample = 101;
        public const int DefaultResumeIdSample = 1;
        public const int DefaultApplicationIdSample = 5001;
        public const int DefaultApplicantNumberSample = 101;

        // App metadata / Console strings (shared with Blazor)
        public const string AppName = "Job Application Tracking System";
        public const string WelcomeMessage = "This system helps you track job postings, monitor applications, and optimize resumes.";
        public const string NoSavedDataMessage = "No saved data found. Creating and saving test user...";
        public const string SavedDataCreatedMessage = "Test user created and saved.";
        public const string LoadedFromDiskMessage = "Loaded test user from disk.";
        public const string TestUserHeader = "Test User Loaded:";
        public const string SavedJobHeader = "Saved Job:";
        public const string ResumeHeader = "Resume Info:";
        public const string ApplicationHeader = "Application Info:";
        public const string ExitPrompt = "Press any key to exit...";

        // Greenhouse / Job Source
        public const string DefaultGreenhouseBoardToken = "vaulttec";


        // Job defaults / labels
        public const int DefaultJobExpirationDays = 30;                  // Used when we don't get a specific expiration
        public const int DefaultJobPostingDaysAgoSample = 3;             // Posted X days ago
        public const int DefaultApplicantsCountForImported = 0;          // Imported jobs unknown applicants
        public const int DefaultApplicantsCountSample = 42;              // Sample data count
        public const string DefaultExperienceLevelEntry = "Entry Level";
        public const string DefaultExperienceLevelUnknown = "Not specified";
        public const string DefaultCurrency = "USD";
        public const string LocationTypeRemote = "Remote";
        public const string LocationTypeOfficeHybrid = "Office/Hybrid";
        public const string LocationNotAvailable = "N/A";
        public const string UnknownLocationLabel = "Unknown";

        // Salary sample data
        public const float DefaultMinSalarySample = 180000f;
        public const float DefaultMaxSalarySample = 250000f;
        public const float DefaultAverageSalarySample = 92000f;

        // Resume / ATS sample data
        public const float DefaultAtsScoreSample = 78.5f;

        // Application timelines sample
        public const int ApplicationDaysAgoSample = 2;         // For example DateTime.Now.AddDays(-2)
        public const int LastFollowUpDaysAgoSample = 1;        // DateTime.Now.AddDays(-1)
        public const int NextFollowUpDaysFromNowSample = 3;    // DateTime.Now.AddDays(+3)

        // Page display (used by Blazor and Logic layers)
        public const int DefaultPage = 1;
        public const int DefaultPageSize = 100;

        // Display
        public const int JobDescriptionPreviewChars = 250;
    }
}