using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /// <summary>
    /// Centralized, reusable constants to avoid magic numbers/strings throughout the codebase.
    /// </summary>
    public static class Constants
    {
        // File names 
        public const string TestDataFileName = "TestData.xml";
        public const string TempSerializationSuffix = ".tempSerializationFile.xml";

        // Common indexes
        public const int FirstIndex = 0;

        // Defaults for sample/test data
        public const int DefaultUserId = 1001;

        // Console strings
        public const string AppName = "Job Application Tracking System";
        public const string WelcomeMessage = "This system helps you track job posting from companies of interest, track application status, and optimize resume.";
        public const string NoSavedDataMessage = "No saved data found. Creating and saving test user...";
        public const string SavedDataCreatedMessage = "Test user created and saved.";
        public const string LoadedFromDiskMessage = "Loaded test user from disk.";
        public const string TestUserHeader = "Test User Loaded:";
        public const string SavedJobHeader = "Saved Job:";
        public const string ResumeHeader = "Resume Info:";
        public const string ApplicationHeader = "Application Info:";
        public const string ExitPrompt = "Press any key to exit...";
    }
}

