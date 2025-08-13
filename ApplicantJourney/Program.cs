using System;

namespace ApplicantJourney
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int FirstIndex = 0; // Constant to represent the first item in a list

            Console.WriteLine("Welcome to the Job Application Tracking System!");
            Console.WriteLine("This system helps you track job posting from companies of interest, track application status, and optimize resume.");
            Console.WriteLine();

            // Initialize test data
            var testData = new TestData();
            var user = testData.CreateTestUser();

            // Display basic user info
            Console.WriteLine("Test User Created:");
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
                Console.WriteLine("Saved Job:");
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
                Console.WriteLine("Resume Info:");
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
                Console.WriteLine("Application Info:");
                Console.WriteLine($"Applied On: {application.ApplicationTime}");
                Console.WriteLine($"Status: {application.Status}");
                Console.WriteLine($"Resume Used: {application.ResumeUsed}");
                Console.WriteLine($"Next Follow-up: {application.NextFollowUpTime}");
                Console.WriteLine($"Platform: {application.SiteWhereUserApplied}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
