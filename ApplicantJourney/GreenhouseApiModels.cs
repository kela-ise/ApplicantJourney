using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("requisition_id")]
        public string RequisitionId { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public GreenhouseLocation? Location { get; set; }

        [JsonPropertyName("absolute_url")]
        public string AbsoluteUrl { get; set; } = string.Empty;

        // Raw vendor HTML content when ?content=true parameter is included
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("metadata")]
        public object Metadata { get; set; } = new object();

        public JobListing ToJobListing(int companyId)
        {
            var locationName = Location?.Name ?? Constants.UnknownLocationLabel;
            var isRemote = locationName.IndexOf("remote", StringComparison.OrdinalIgnoreCase) >= 0;

            return new JobListing
            {
                Id = Id,
                JobTitle = Title,
                Company = companyId,
                JobPostingDate = UpdatedAt,
                JobExpirationDate = UpdatedAt.AddDays(Constants.DefaultJobExpirationDays),
                // Store plain text version for consistent display across renderers
                JobDescriptionHtml = HtmlText.ToPlainText(Content ?? string.Empty),
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
    }

    public class GreenhouseLocation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class GreenhouseMeta
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}