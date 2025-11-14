using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blazor1ApplicantJourney.Data;

namespace Blazor1ApplicantJourney.Data
{
    public class Blazor1ApplicantJourneyContext(DbContextOptions<Blazor1ApplicantJourneyContext> options) : IdentityDbContext<Blazor1ApplicantJourneyUser>(options)
    {
    }
}
