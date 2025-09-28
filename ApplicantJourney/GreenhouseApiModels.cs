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

        // Present when ?content=true is used
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("metadata")]
        public object Metadata { get; set; }

        public JobListing ToJobListing(int companyId)
        {
            var locationName = Location?.Name ?? Constants.UnknownLocationLabel;
            var isRemote = locationName.IndexOf("remote", StringComparison.OrdinalIgnoreCase) >= 0;

            // Preserve raw HTML (full original API data). No plain-text stored.
            var rawHtml = Content ?? string.Empty;

            return new JobListing
            {
                Id = Id,
                JobTitle = Title,
                Company = companyId,
                JobPostingDate = UpdatedAt,
                JobExpirationDate = UpdatedAt.AddDays(Constants.DefaultJobExpirationDays),
                JobDescriptionHtml = rawHtml, // keep original description as-is
                ExperienceLevel = Constants.DefaultExperienceLevelUnknown,
                Source = JobListingSource.CompanyWebsite,
                Url = AbsoluteUrl,
                ApplicantsCount = Constants.DefaultApplicantsCountForImported,
                Salary = new SalaryRange { Min = 0, Max = 0, Currency = Constants.DefaultCurrency },
                JobLocation = new LocationInfo
                {
                    LocationType = isRemote ? Constants.LocationTypeRemote : Constants.LocationTypeOfficeHybrid,
                    IsRemote = isRemote,
                    IsHybrid = !isRemote && locationName.IndexOf("hybrid", StringComparison.OrdinalIgnoreCase) >= 0,
                    PhysicalLocation = locationName
                },
                Type = JobType.FullTime
            };
        }

        /// <summary>
        /// Utility: convert HTML to plain text for display contexts (does not mutate stored data).
        /// </summary>
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

        /// <summary>
        /// Utility: first N chars of plain text for previews (does not mutate stored data).
        /// </summary>
        public static string GetPlainTextPreview(string? html, int maxChars)
        {
            var text = GetPlainText(html);
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return text.Length <= maxChars ? text : text.Substring(0, maxChars) + "...";
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

