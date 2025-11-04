using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ProjectsController : Controller
    {
        // Hardcoded projects list with your COMMUNIZEN project
        private static List<Project> _projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Title = "COMMUNIZEN - Mental Health Support Platform",
                Description = "Developed an innovative cross-platform mobile application providing mental health support, resources, and professional connections with focus on accessibility, security, and user experience. " +
                             "The application features real-time chat functionality for immediate counselor connection, an appointment booking system with calendar integration, " +
                             "and personalized wellness plans with AI-driven recommendations. Built using .NET MAUI with MVVM architecture, Firebase backend for authentication " +
                             "and real-time data synchronization, and Google Maps API integration for location-based practitioner search.",
                ImageUrl = "/Images/communizen.png", // Update with actual image path
                ProjectUrl = "https://drive.google.com/file/d/1COjWWSFxzrMRuFK-aDN2KE6d89i7iYel/view?usp=drivesdk", // Add if you have a live demo
                GitHubUrl = "https://drive.google.com/file/d/1COjWWSFxzrMRuFK-aDN2KE6d89i7iYel/view?usp=drivesdk",
                Technologies = new List<string>
                {
                    ".NET MAUI",
                    "C#",
                    "MVVM Pattern",
                    "XAML",
                    "Firebase Authentication",
                    "Firestore Database",
                    "Google Maps API",
                    "RESTful API",
                    "Real-time Chat",
                    "Cross-Platform Development"
                },
                CompletionDate = new DateTime(2024, 12, 1),
                IsFeatured = true
            },
            // Add more projects as needed
            new Project
            {
                Id = 2,
                Title = "Portfolio Website",
                Description = "A responsive personal portfolio website built with ASP.NET Core MVC showcasing projects, skills, and professional experience. " +
                             "Features include dynamic project filtering, admin dashboard for content management, and modern UI design with Bootstrap.",
                ImageUrl = "/Images/portfolio.png",
                ProjectUrl = "", // Your portfolio URL
                GitHubUrl = "https://github.com/ManwathaW/MyPortfolio.git", // Your portfolio GitHub URL
                Technologies = new List<string>
                {
                    "ASP.NET Core MVC",
                    "C#",
                    "Entity Framework",
                    "SQLite",
                    "Bootstrap",
                    "HTML5",
                    "CSS3",
                    "JavaScript"
                },
                CompletionDate = new DateTime(2025, 1, 15),
                IsFeatured = true
            }
        };

        public IActionResult Index(string technology = null, string viewMode = "grid", int page = 1, int pageSize = 4)
        {
            // Get all unique technologies for the filter dropdown
            var allTechnologies = _projects
                .SelectMany(p => p.Technologies)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            IEnumerable<Project> filteredProjects;

            // Filter projects by technology if selected
            if (!string.IsNullOrEmpty(technology))
            {
                filteredProjects = _projects
                    .Where(p => p.Technologies.Contains(technology))
                    .ToList();
            }
            else
            {
                filteredProjects = _projects;
            }

            // Calculate pagination values
            int totalItems = filteredProjects.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            // Get the current page's projects
            var pagedProjects = filteredProjects
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.CompletionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create a view model to pass all necessary data to the view
            var viewModel = new ProjectsViewModel
            {
                Projects = pagedProjects,
                Technologies = allTechnologies,
                SelectedTechnology = technology,
                ViewMode = viewMode,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}