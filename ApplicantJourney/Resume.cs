using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    /*
    Users resume and ATS analysis data
    */
    internal class Resume
    {

        public int ResumeId { get; set; } // Unique resume ID
        public Guid UserId { get; set; }  // ID of the user who uploaded it
        public string FileUrl { get; set; }   // URL or path where the resume file is stored
        public DateTime ResumeUploadTime { get; set; }
        public float AtsScore { get; set; }  // ATS score analysis result
        public List<string> MissingKeywords { get; set; }  // Keywords missing from resume
        public List<string> FormatIssues { get; set; }
        public List<JobMatch> JobMatches { get; set; }

        public Resume()
        {
            MissingKeywords = new List<string>();
            FormatIssues = new List<string>();
            JobMatches = new List<JobMatch>();
        }
    }

    public class JobMatch
    {
        public int JobId { get; set; }
        public float MatchPercentage { get; set; }
        public string SuggestedImprovements { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }

    }
}
