using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ApplicantJourney
{
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
        public GreenhouseLocation Location { get; set; }

        [JsonPropertyName("absolute_url")]
        public string AbsoluteUrl { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } // job description (HTML/markdown)
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

    public class GreenhouseRoot
    {
        [JsonPropertyName("jobs")]
        public List<GreenhouseJob> Jobs { get; set; }

        [JsonPropertyName("meta")]
        public GreenhouseMeta Meta { get; set; }
    }
}
