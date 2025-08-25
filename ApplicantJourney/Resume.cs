using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Users resume and ATS analysis data
*/
namespace ApplicantJourney
{
    internal class Resume
    {
        public int Id { get; set; } // Unique resume ID
        public int User { get; set; }  // ID of the user who uploaded it
        public string FileUrl { get; set; }   // URL or path where the resume file is stored
        public DateTime ResumeUploadTime { get; set; }
        public float AtsScore { get; set; }  // ATS score analysis result
        public List<string> MissingKeywords { get; set; } = new List<string>();  // Keywords missing from resume
        public List<string> FormatIssues { get; set; } = new List<string>();
        public List<JobMatch> JobMatches { get; set; } = new List<JobMatch>();
    }
    public class JobMatch
    {
        public int Id { get; set; }
        public float MatchPercentage { get; set; }
        public string SuggestedImprovements { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
    }
}
