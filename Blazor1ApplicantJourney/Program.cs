using Blazor1ApplicantJourney.Components;
using ApplicantJourney;
using Microsoft.AspNetCore.Components;

namespace Blazor1ApplicantJourney
{
    public class Program
    {
        /// <summary>
        /// Basic Blazor app setup.
        /// Business logic can be found in ApplicantJourney (Logic.cs, JobSearchService, etc.).
        /// Updated to include CompanyConfiguration.
        /// </summary>
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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