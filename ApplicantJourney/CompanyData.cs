using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    internal class CompanyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HiringTrends { get; set; } // Trend data for job postings or hiring
        public float AverageSalary { get; set; } // Estimated salary for roles
        public string JobSource { get; set; }  // Where job was sourced (e.g., LinkedIn)
        public float ReputationScore { get; set; }      // Public/company ratings (e.g., from Glassdoor)
        public int EstimatedApplications { get; set; }  // How many applicants typically apply
        public string CompetitionInsights { get; set; } // Notes on competition or demand


    }
}
