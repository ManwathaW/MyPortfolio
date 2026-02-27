using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;
using Portfolio.Services;
using System;
using System.Collections.Generic;

namespace Portfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add HttpClientFactory for external API calls
            builder.Services.AddHttpClient();

            // Register Enhancement Services (TIER 3 & TIER 4)
            builder.Services.AddScoped<PortfolioEnhancementService>();
            builder.Services.AddScoped<UIUXEnhancementService>();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

            // Add Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Account/Login";
                options.AccessDeniedPath = "/Admin/Account/AccessDenied";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapCustomRoutes();

            // Create and seed the database if it doesn't exist
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Check if database exists and create it if it doesn't
                    context.Database.EnsureCreated();

                    // Check if we need to seed a sample project (only if no projects exist)
                    if (!context.Projects.Any())
                    {
                        // Add a sample project
                        var sampleProject = new Project
                        {
                            Title = "Sample Portfolio Project",
                            Description = "This is a sample project created automatically when your portfolio was deployed.",
                            ImageUrl = "/images/projects/default-project.jpg",
                            ProjectUrl = "https://example.com",
                            GitHubUrl = "https://github.com/yourusername/portfolio",
                            Technologies = new List<string> { "ASP.NET Core", "C#", "SQLite", "Bootstrap" },
                            CompletionDate = DateTime.Now,
                            IsFeatured = true
                        };

                        context.Projects.Add(sampleProject);
                        context.SaveChanges();

                        Console.WriteLine("Added sample project to database");
                    }

                    // Seed admin user and role
                    SeedAdminUser(userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");

                    // Also log to console for deployment logs
                    Console.WriteLine($"Database error: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }

            app.Run();

            // Helper method to seed admin user
            async Task SeedAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                // Create admin role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create admin user if it doesn't exist
                var adminEmail = "manwathawanga312@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    // Change this to a secure password in production
                    var result = await userManager.CreateAsync(adminUser, "Admin@123456");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
        }
    }
}