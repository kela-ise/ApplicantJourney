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

            Console.Write($"Enter a Greenhouse board token (default = {DefaultGreenhouseBoardToken}): ");
            var inputToken = Console.ReadLine();
            var boardToken = string.IsNullOrWhiteSpace(inputToken)
                ? DefaultGreenhouseBoardToken
                : inputToken.Trim();

            Console.WriteLine("\nFetching live job listings from Greenhouse (full HTML descriptions)...");
            Console.WriteLine($"Greenhouse API URL: https://boards-api.greenhouse.io/v1/boards/{boardToken}/jobs?content=true");

            try
            {
                var logic = new Logic();
                var jobs = logic.RunPipelineAsync(
                    boardToken: boardToken,
                    companyId: 0,
                    includeContent: true,
                    sortBy: "date",
                    sortDesc: true,
                    page: DefaultPage,
                    pageSize: DefaultPageSize
                ).GetAwaiter().GetResult();

                foreach (var job in jobs)
                {
                    Console.WriteLine($"- {job.JobTitle} | {job.JobLocation?.PhysicalLocation}");
                    Console.WriteLine($"  {job.Url}");
                    Console.WriteLine("  Description:");
                    Console.WriteLine(job.JobDescriptionHtml);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
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