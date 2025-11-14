using Blazor1ApplicantJourney.Data;
using Microsoft.AspNetCore.Identity;

namespace Blazor1ApplicantJourney.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<Blazor1ApplicantJourneyUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<Blazor1ApplicantJourneyUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
