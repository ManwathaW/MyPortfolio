using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Portfolio.Models;

namespace Portfolio.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Project entity to handle the Technologies list with proper value comparer
            modelBuilder.Entity<Project>()
                .Property(p => p.Technologies)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    )
                );

            // Seed some initial data if needed
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed an admin user
            // Note: In production, use a secure password and proper hashing

            // Seed some initial projects
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Title = "PROFILE",
                    Description = "A full-stack e-commerce solution with shopping cart and payment integration.",
                    ImageUrl = "/images/projects/ecommerce.jpg",
                    ProjectUrl = "https://example.com/ecommerce",
                    GitHubUrl = "https://github.com/yourusername/ecommerce",
                    Technologies = new List<string> { "ASP.NET Core", "C#", "Entity Framework", "SQL Server", "JavaScript", "Bootstrap" },
                    CompletionDate = new DateTime(2023, 6, 15),
                    IsFeatured = true
                }
            );
        }
    }
}


