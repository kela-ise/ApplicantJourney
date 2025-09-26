using System;
using System.IO;
using System.Threading.Tasks;
using static ApplicantJourney.Constants;


namespace ApplicantJourney
{
    internal class Program
    {
        static async Task Main(string[] args)
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

            // Prompt for Greenhouse board token
            Console.Write("Enter a Greenhouse board token (default = vaulttec): ");
            var inputToken = Console.ReadLine();
            var boardToken = string.IsNullOrWhiteSpace(inputToken) ? "vaulttec" : inputToken.Trim();

            var greenhouseClient = new GreenhouseApiClient();
            Console.WriteLine("\nFetching live job listings from Greenhouse...");

            try
            {
                var jobsResponse = await greenhouseClient.FetchJobsAsync(boardToken);

                if (jobsResponse?.Jobs != null && jobsResponse.Jobs.Count > 0)
                {
                    Console.WriteLine($"Found {jobsResponse.Jobs.Count} jobs:\n");

                    foreach (var job in jobsResponse.Jobs)
                    {
                        // Map API job into our JobListing structure
                        var mappedJob = new JobListing
                        {
                            Id = job.Id,
                            JobTitle = job.Title,
                            JobDescription = job.Content, // using job.content for description
                            Company = 0, // Greenhouse API doesn’t provide numeric company id
                            JobPostingDate = job.UpdatedAt,
                            JobExpirationDate = DateTime.Now.AddDays(30), // placeholder
                            Source = JobListingSource.Other,
                            Url = job.AbsoluteUrl,
                            ApplicantsCount = 0,
                            ExperienceLevel = "Not Specified",
                            Salary = new SalaryRange
                            {
                                Min = 0,
                                Max = 0,
                                Currency = "USD"
                            },
                            JobLocation = new LocationInfo
                            {
                                LocationType = "Office/Remote",
                                IsRemote = false,
                                IsHybrid = false,
                                PhysicalLocation = job.Location?.Name ?? "Unknown"
                            },
                            Type = JobType.FullTime
                        };

                        // Print mapped job details
                        Console.WriteLine($"- {mappedJob.JobTitle} | {mappedJob.JobLocation.PhysicalLocation}");
                        Console.WriteLine($"  {mappedJob.Url}");
                        Console.WriteLine($"  Description: {mappedJob.JobDescription}\n");
                    }
                }
                else
                {
                    Console.WriteLine("No jobs found or API request failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {ex.Message}");
            }

            testData.SaveTestUser(user);
            Console.WriteLine();
            Console.WriteLine("Current state saved to disk.");

            Console.WriteLine();
            Console.WriteLine(ExitPrompt);
            Console.ReadKey();
        }
    }
}
