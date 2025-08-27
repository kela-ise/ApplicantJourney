using System;
using System.IO;
using static ApplicantJourney.Constants; 

namespace ApplicantJourney
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to the {AppName}!");
            Console.WriteLine(WelcomeMessage);
            Console.WriteLine();

            // Show my data file location
            Console.WriteLine($"Data file path: {Path.GetFullPath(TestDataFileName)}");
            Console.WriteLine();

            // Initialize test data helper
            var testData = new TestData();

            // Try to load persisted test data first; if not found, create and persist it
            var user = testData.LoadTestUser();
            if (user == null)
            {
                Console.WriteLine(NoSavedDataMessage);
                user = testData.CreateTestUser();
                Console.WriteLine(SavedDataCreatedMessage);
            }
            else
            {
                Console.WriteLine(LoadedFromDiskMessage);
            }

            Console.WriteLine();

            // Display basic user info
            Console.WriteLine(TestUserHeader);
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Preferred Roles: {string.Join(", ", user.Preferences.PreferredJobTitles)}");
            Console.WriteLine($"Preferred Locations: {string.Join(", ", user.Preferences.Locations)}");
            Console.WriteLine();

            // Display saved job listing if available
            bool hasSavedJobs = user.SavedPositions.Count > FirstIndex; 
            if (hasSavedJobs)
            {
                var savedJob = user.SavedPositions[FirstIndex];
                Console.WriteLine(SavedJobHeader);
                Console.WriteLine($"Title: {savedJob.JobTitle}");
                Console.WriteLine($"Company ID: {savedJob.Company}");
                Console.WriteLine($"Description: {savedJob.JobDescription}");
                Console.WriteLine($"Salary Range: {savedJob.Salary.Min} - {savedJob.Salary.Max} {savedJob.Salary.Currency}");
                Console.WriteLine($"Posted: {savedJob.JobPostingDate.ToShortDateString()}");
                Console.WriteLine($"URL: {savedJob.Url}");
                Console.WriteLine();
            }

            bool myUploadedResumes = user.Resumes.Count > FirstIndex; // Display resume details if any resume is uploaded
            if (myUploadedResumes)
            {
                var resume = user.Resumes[FirstIndex];
                Console.WriteLine(ResumeHeader);
                Console.WriteLine($"Uploaded: {resume.ResumeUploadTime}");
                Console.WriteLine($"ATS Score: {resume.AtsScore}");
                Console.WriteLine($"Missing Keywords: {string.Join(", ", resume.MissingKeywords)}");
                Console.WriteLine($"Format Issues: {string.Join(", ", resume.FormatIssues)}");
                Console.WriteLine();
            }

            // Display application details if user has tracked applications
            bool hasTrackedApplications = user.TrackedApplications.Count > FirstIndex;
            if (hasTrackedApplications)
            {
                var application = user.TrackedApplications[FirstIndex];
                Console.WriteLine(ApplicationHeader);
                Console.WriteLine($"Applied On: {application.ApplicationTime}");
                Console.WriteLine($"Status: {application.Status}");
                Console.WriteLine($"Resume Used: {application.ResumeUsed}");
                Console.WriteLine($"Next Follow-up: {application.NextFollowUpTime}");
                Console.WriteLine($"Platform: {application.SiteWhereUserApplied}");
            }

            // Always persist current state before exit (even if we only loaded)
            testData.SaveTestUser(user);
            Console.WriteLine();
            Console.WriteLine("Current state saved to disk.");

            Console.WriteLine();
            Console.WriteLine(ExitPrompt);
            Console.ReadKey();
        }
    }
}
