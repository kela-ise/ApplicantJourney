using Blazor1ApplicantJourney.Components;
using ApplicantJourney;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blazor1ApplicantJourney.Data;

namespace Blazor1ApplicantJourney
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Adding database context with SQLite for development
            builder.Services.AddDbContext<Blazor1ApplicantJourneyContext>(options =>
                options.UseSqlite("Data Source=app.db"));

            // Add authentication services first
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            })
            .AddIdentityCookies();

            // Adding Identity with Entity Framework
            builder.Services.AddIdentityCore<Blazor1ApplicantJourneyUser>(options =>
            {
                // eeping user management simple for now
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
            .AddEntityFrameworkStores<Blazor1ApplicantJourneyContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            // Adding authentication and authorization
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddAuthorization();

            // Add Identity supporting services
            builder.Services.AddSingleton<IEmailSender<Blazor1ApplicantJourneyUser>, Blazor1ApplicantJourney.Components.Account.IdentityNoOpEmailSender>();
            builder.Services.AddScoped<Blazor1ApplicantJourney.Components.Account.IdentityRedirectManager>();
            builder.Services.AddScoped<Blazor1ApplicantJourney.Components.Account.IdentityUserAccessor>();

            // Connect to ApplicantJourney logic layer 
            builder.Services.AddSingleton<JobSearchService>();
            builder.Services.AddSingleton<IJobSource, GreenhouseSource>();
            builder.Services.AddTransient<Logic>(sp =>
                new Logic(
                    sp.GetRequiredService<IJobSource>(),
                    sp.GetRequiredService<JobSearchService>()
                )
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            // Add authentication middleware - CRITICAL!
            app.UseAuthentication();
            app.UseAuthorization();

            // Create database on startup (development only)
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Blazor1ApplicantJourneyContext>();
                dbContext.Database.EnsureCreated(); // Auto-creates tables
            }

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints for Identity pages
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}