using Blazor1ApplicantJourney.Components;
using ApplicantJourney;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;

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

            // Connect to ApplicantJourney logic layer 
            builder.Services.AddSingleton<JobSearchService>();
            builder.Services.AddSingleton<IJobSource, GreenhouseSource>();
            builder.Services.AddTransient<Logic>(sp =>
                new Logic(
                    sp.GetRequiredService<IJobSource>(),
                    sp.GetRequiredService<JobSearchService>()
                )
            );

            // Add authentication and authorization services if needed
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddAuthorization();

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

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}