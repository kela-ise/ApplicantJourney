using System;
using System.IO;
using System.Threading.Tasks;
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

            Console.WriteLine($"Data file path: {Path.GetFullPath(TestDataFileName)}");
            Console.WriteLine();

            var testData = new TestData();
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
            Console.WriteLine(TestUserHeader);
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Preferred Roles: {string.Join(", ", user.Preferences.PreferredJobTitles)}");
            Console.WriteLine($"Preferred Locations: {string.Join(", ", user.Preferences.Locations)}");
            Console.WriteLine();

            // Ask for Greenhouse board token
            Console.Write($"Enter a Greenhouse board token (default = {DefaultGreenhouseBoardToken}): ");
            var inputToken = Console.ReadLine();
            var boardToken = string.IsNullOrWhiteSpace(inputToken) ? DefaultGreenhouseBoardToken : inputToken.Trim();

            var greenhouseClient = new GreenhouseApiClient();
            Console.WriteLine("\nFetching live job listings from Greenhouse (with HTML descriptions)...");
            Console.WriteLine($"Greenhouse API URL: https://boards-api.greenhouse.io/v1/boards/{boardToken}/jobs?content=true");

            try
            {
                var jobsResponse = greenhouseClient
                    .FetchJobsAsync(boardToken, includeContent: true)
                    .GetAwaiter()
                    .GetResult();

                if (jobsResponse?.Jobs != null && jobsResponse.Jobs.Count > 0)
                {
                    Console.WriteLine($"Found {jobsResponse.Meta?.Total ?? jobsResponse.Jobs.Count} jobs:\n");

                    foreach (var greenhouseJob in jobsResponse.Jobs)
                    {
                        // Map API job into our domain model (keeps raw HTML only)
                        var mapped = greenhouseJob.ToJobListing(companyId: 0);

                        Console.WriteLine($"- {mapped.JobTitle} | {mapped.JobLocation.PhysicalLocation}");
                        Console.WriteLine($"  {mapped.Url}");

                        // Compute preview on-the-fly for console readability (plain text derived from raw HTML).
                        var preview = GreenhouseJob.GetPlainTextPreview(mapped.JobDescriptionHtml, JobDescriptionPreviewChars);
                        Console.WriteLine($"  Description (first {JobDescriptionPreviewChars} chars, plain text): {preview}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No jobs found or API request failed.");
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Tip: verify the token in a browser: https://boards.greenhouse.io/<token>");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUnexpected error while fetching jobs:");
                Console.WriteLine(ex.Message);
            }

            // Persist current state before exit
            testData.SaveTestUser(user);
            Console.WriteLine();
            Console.WriteLine("Current state saved to disk.");

            Console.WriteLine();
            Console.WriteLine(ExitPrompt);
            Console.ReadKey();
        }
    }
}
