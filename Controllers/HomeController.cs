using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Hardcoded projects list
        private static List<Project> _projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Title = "COMMUNIZEN - Mental Health Support Platform",
                Description = "Innovative cross-platform mobile application providing comprehensive mental health support with real-time chat, appointment booking, and AI-driven wellness plans.",
                ImageUrl = "/images/communizen.png",
                ProjectUrl = "",
                GitHubUrl = "https://github.com/ManwathaW/CommunizenV2.git",
                Technologies = new List<string>
                {
                    ".NET MAUI",
                    "C#",
                    "MVVM Pattern",
                    "XAML",
                    "Firebase",
                    "Google Maps API"
                },
                CompletionDate = new DateTime(2024, 12, 1),
                IsFeatured = true
            },
            new Project
            {
                Id = 2,
                Title = "Portfolio Website",
                Description = "Responsive personal portfolio website built with ASP.NET Core MVC showcasing projects, skills, and professional experience with admin dashboard.",
                ImageUrl = "/images/portfolio.png",
                ProjectUrl = "",
                GitHubUrl = "",
                Technologies = new List<string>
                {
                    "ASP.NET Core MVC",
                    "C#",
                    "Bootstrap",
                    "SQLite"
                },
                CompletionDate = new DateTime(2025, 1, 15),
                IsFeatured = true
            }
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Your personal information
            ViewData["Name"] = "MANWATHA WANGA COMFORT";
            ViewData["Title"] = "Full Stack Developer";
            ViewData["Summary"] = "Results-driven Full Stack Developer with expertise in .NET, PHP, and WordPress, " +
                "specializing in scalable web and mobile solutions using C#, ASP.NET Core MVC, and CMS platforms. " +
                "Awarded 2nd Place Overall Top Trainee (LDIL Programme 2025) and 2nd Place Winner (Limpopo Connection Hackathon 2024). " +
                "Passionate about leveraging cloud technologies, AI, and machine learning to solve real-world problems.";

            // Contact information
            ViewData["Email"] = "Manwathawanga312@gmail.com";
            ViewData["Phone"] = "(+27) 69 910 5737";
            ViewData["LinkedIn"] = "linkedin.com/in/wanga-manwatha-112wc";
            ViewData["GitHub"] = "github.com/ManwathaW";
            ViewData["Location"] = "Johannesburg, Gauteng, South Africa";

            // Achievements
            ViewData["Achievement1"] = "🏆 2nd Place Overall Top Trainee - LDIL Programme 2025";
            ViewData["Achievement2"] = "🏆 2nd Place Winner - Limpopo Connection Hackathon 2024";

            // Get featured projects for the homepage
            var featuredProjects = _projects
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.CompletionDate)
                .Take(2)
                .ToList();

            ViewBag.FeaturedProjects = featuredProjects;

            return View();
        }

        public IActionResult About()
        {
            // About page information
            ViewData["Name"] = "MANWATHA WANGA COMFORT";
            ViewData["Title"] = "Full Stack Developer";

            ViewData["AboutMe"] = "I am a results-driven Full Stack Developer with a BSc in Computer Science and hands-on experience " +
                "in building scalable web applications using .NET, PHP, and WordPress. My journey in software development has been marked " +
                "by recognition and achievement, including being awarded 2nd Place Overall Top Trainee in the LDIL Programme 2025 and " +
                "2nd Place Winner at the Limpopo Connection Hackathon 2024.";

            ViewData["Passion"] = "I am passionate about using technology to solve real-world challenges, particularly in areas that " +
                "create social impact. My flagship project, COMMUNIZEN, demonstrates this commitment by providing a comprehensive mental " +
                "health support platform. I continuously explore emerging technologies like Machine Learning, AI, and cloud computing to " +
                "stay at the forefront of software development.";

            ViewData["CurrentRole"] = "Currently working as a Software Developer at ORZOFLASH (Pty) Ltd, where I develop responsive web " +
                "applications using ASP.NET Core MVC, PHP, and WordPress. I also completed an intensive 12-month Advanced Software Development " +
                "internship at the University of Limpopo's Digital Innovation Lab, where I honed my skills in multiplatform software development.";

            // Key Skills for About page
            ViewData["KeySkill1"] = "Full Stack Development (.NET & PHP)";
            ViewData["KeySkill2"] = "WordPress & CMS Development";
            ViewData["KeySkill3"] = "Mobile App Development (.NET MAUI)";
            ViewData["KeySkill4"] = "Cloud Technologies (Azure, Firebase)";
            ViewData["KeySkill5"] = "Database Design & Management";
            ViewData["KeySkill6"] = "RESTful API Development";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}