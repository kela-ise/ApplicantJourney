using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantJourney
{
    public class CompanyData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HiringTrends { get; set; } = string.Empty; // Trend data for job postings or hiring
        public float AverageSalary { get; set; } // Estimated salary for roles
        public string JobSource { get; set; } = string.Empty;  // Where job was sourced (e.g., LinkedIn)
        public float ReputationScore { get; set; }      // Public/company ratings (e.g., from Glassdoor)
        public int EstimatedApplications { get; set; }  // How many applicants typically apply
        public string CompetitionInsights { get; set; } = string.Empty; // Notes on competition or demand

        // Greenhouse-specific properties
        public string BoardToken { get; set; } = string.Empty; // Greenhouse board token for API calls
    }

    /// <summary>
    /// Static configuration for companies and their Greenhouse board tokens
    /// </summary>
    public static class CompanyConfiguration
    {
        public static readonly IReadOnlyList<CompanyData> Companies = new[]
        {
            new CompanyData
            {
                Id = 1,
                Name = "Asana",
                BoardToken = "asana",
                HiringTrends = "Growing rapidly in product and engineering roles",
                AverageSalary = 145000f,
                JobSource = "Greenhouse",
                ReputationScore = 4.5f,
                EstimatedApplications = 200,
                CompetitionInsights = "High competition for product roles"
            },
            new CompanyData
            {
                Id = 2,
                Name = "Gusto",
                BoardToken = "gusto",
                HiringTrends = "Expanding in fintech and payroll services",
                AverageSalary = 135000f,
                JobSource = "Greenhouse",
                ReputationScore = 4.3f,
                EstimatedApplications = 150,
                CompetitionInsights = "Strong in financial technology sector"
            },
            new CompanyData
            {
                Id = 3,
                Name = "Robinhood",
                BoardToken = "robinhood",
                HiringTrends = "Hiring in financial technology and mobile development",
                AverageSalary = 155000f,
                JobSource = "Greenhouse",
                ReputationScore = 4.1f,
                EstimatedApplications = 180,
                CompetitionInsights = "Competitive mobile engineering market"
            },
            new CompanyData
            {
                Id = 4,
                Name = "Vaulttec",
                BoardToken = "vaulttec",
                HiringTrends = "Steady growth in security and infrastructure roles",
                AverageSalary = 120000f,
                JobSource = "Greenhouse",
                ReputationScore = 4.0f,
                EstimatedApplications = 80,
                CompetitionInsights = "Niche market with specialized roles"
            }
        };

        public static CompanyData? GetCompanyByToken(string boardToken)
        {
            return Companies.FirstOrDefault(company =>
                string.Equals(company.BoardToken, boardToken, StringComparison.OrdinalIgnoreCase));
        }

        public static CompanyData? GetCompanyById(int companyId)
        {
            return Companies.FirstOrDefault(company => company.Id == companyId);
        }

        public static CompanyData? GetCompanyByName(string companyName)
        {
            return Companies.FirstOrDefault(company =>
                string.Equals(company.Name, companyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}