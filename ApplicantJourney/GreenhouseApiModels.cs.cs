using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ApplicantJourney
{
    public class GreenhouseRoot
    {
        [JsonPropertyName("jobs")]
        public List<GreenhouseJob> Jobs { get; set; } = new();

        [JsonPropertyName("meta")]
        public GreenhouseMeta? Meta { get; set; }
    }

    public class GreenhouseJob
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("internal_job_id")]
        public int InternalJobId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("requisition_id")]
        public string RequisitionId { get; set; }

        [JsonPropertyName("location")]
        public GreenhouseLocation? Location { get; set; }

        [JsonPropertyName("absolute_url")]
        public string AbsoluteUrl { get; set; }

        // Only present when ?content=true is used
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("metadata")]
        public object Metadata { get; set; }

        public JobListing ToJobListing(int companyId)
        {
            var locationName = Location?.Name ?? "Unknown";
            var isRemote = locationName.IndexOf("remote", StringComparison.OrdinalIgnoreCase) >= 0;

            // Use plain text for description to keep console readable
            var description = GetPlainText(Content);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = $"Imported from Greenhouse (Req: {RequisitionId})";
            }

            return new JobListing
            {
                Id = Id,
                JobTitle = Title,
                Company = companyId,
                JobPostingDate = UpdatedAt,
                JobExpirationDate = UpdatedAt.AddDays(30),
                JobDescription = description,
                ExperienceLevel = "Not specified",
                Source = JobListingSource.CompanyWebsite,
                Url = AbsoluteUrl,
                ApplicantsCount = 0,
                Salary = new SalaryRange { Min = 0, Max = 0, Currency = "USD" },
                JobLocation = new LocationInfo
                {
                    LocationType = isRemote ? "Remote" : "Office/Hybrid",
                    IsRemote = isRemote,
                    IsHybrid = !isRemote && locationName.IndexOf("hybrid", StringComparison.OrdinalIgnoreCase) >= 0,
                    PhysicalLocation = locationName
                },
                Type = JobType.FullTime
            };
        }

        public static string GetPlainTextPreview(string? html, int maxChars)
        {
            var text = GetPlainText(html);
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            text = Regex.Replace(text, @"\s+", " ").Trim();
            return text.Length <= maxChars ? text : text.Substring(0, maxChars) + "...";
        }

        public static string GetPlainText(string? html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var noScript = Regex.Replace(html, @"<script[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            var noStyle = Regex.Replace(noScript, @"<style[\s\S]*?</style>", "", RegexOptions.IgnoreCase);
            var noTags = Regex.Replace(noStyle, "<.*?>", " ");
            string text = noTags
                .Replace("&nbsp;", " ")
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&#39;", "'");
            return Regex.Replace(text, @"\s+", " ").Trim();
        }
    }

    public class GreenhouseLocation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class GreenhouseMeta
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
