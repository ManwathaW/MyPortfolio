using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class SkillsController : Controller
    {
        private static List<Skill> _skills = new List<Skill>
        {
            // Programming Languages
            new Skill { Id = 1, Name = "C#", ProficiencyLevel = 5, Category = "Programming Languages" },
            new Skill { Id = 2, Name = "PHP", ProficiencyLevel = 4, Category = "Programming Languages" },
            new Skill { Id = 3, Name = "Python", ProficiencyLevel = 4, Category = "Programming Languages" },
            new Skill { Id = 4, Name = "Java", ProficiencyLevel = 3, Category = "Programming Languages" },
            new Skill { Id = 5, Name = "JavaScript", ProficiencyLevel = 4, Category = "Programming Languages" },
            new Skill { Id = 6, Name = "SQL", ProficiencyLevel = 4, Category = "Programming Languages" },
            
            // Backend Frameworks & Technologies
            new Skill { Id = 7, Name = ".NET Core", ProficiencyLevel = 5, Category = "Backend Frameworks" },
            new Skill { Id = 8, Name = "ASP.NET Core MVC", ProficiencyLevel = 5, Category = "Backend Frameworks" },
            new Skill { Id = 9, Name = "ASP.NET Razor Pages", ProficiencyLevel = 4, Category = "Backend Frameworks" },
            new Skill { Id = 10, Name = ".NET MAUI", ProficiencyLevel = 4, Category = "Backend Frameworks" },
            new Skill { Id = 11, Name = "Entity Framework", ProficiencyLevel = 4, Category = "Backend Frameworks" },
            new Skill { Id = 12, Name = "Laravel (PHP)", ProficiencyLevel = 3, Category = "Backend Frameworks" },
            
            // CMS & WordPress
            new Skill { Id = 13, Name = "WordPress Development", ProficiencyLevel = 4, Category = "CMS & WordPress" },
            new Skill { Id = 14, Name = "Theme Customization", ProficiencyLevel = 4, Category = "CMS & WordPress" },
            new Skill { Id = 15, Name = "Plugin Development", ProficiencyLevel = 4, Category = "CMS & WordPress" },
            new Skill { Id = 16, Name = "WooCommerce", ProficiencyLevel = 4, Category = "CMS & WordPress" },
            new Skill { Id = 17, Name = "WordPress REST API", ProficiencyLevel = 3, Category = "CMS & WordPress" },
            
            // Frontend Technologies
            new Skill { Id = 18, Name = "HTML5", ProficiencyLevel = 5, Category = "Frontend" },
            new Skill { Id = 19, Name = "CSS3", ProficiencyLevel = 5, Category = "Frontend" },
            new Skill { Id = 20, Name = "Bootstrap", ProficiencyLevel = 4, Category = "Frontend" },
            new Skill { Id = 21, Name = "jQuery", ProficiencyLevel = 4, Category = "Frontend" },
            new Skill { Id = 22, Name = "XAML", ProficiencyLevel = 4, Category = "Frontend" },
            new Skill { Id = 23, Name = "Responsive Design", ProficiencyLevel = 5, Category = "Frontend" },
            
            // Web Development & APIs
            new Skill { Id = 24, Name = "RESTful API", ProficiencyLevel = 4, Category = "Web Development" },
            new Skill { Id = 25, Name = "Web API", ProficiencyLevel = 4, Category = "Web Development" },
            new Skill { Id = 26, Name = "Microservices", ProficiencyLevel = 3, Category = "Web Development" },
            new Skill { Id = 27, Name = "MVC Pattern", ProficiencyLevel = 5, Category = "Web Development" },
            new Skill { Id = 28, Name = "MVVM Pattern", ProficiencyLevel = 4, Category = "Web Development" },
            
            // Databases
            new Skill { Id = 29, Name = "SQL Server", ProficiencyLevel = 4, Category = "Databases" },
            new Skill { Id = 30, Name = "MySQL", ProficiencyLevel = 4, Category = "Databases" },
            new Skill { Id = 31, Name = "PostgreSQL", ProficiencyLevel = 3, Category = "Databases" },
            new Skill { Id = 32, Name = "SQLite", ProficiencyLevel = 4, Category = "Databases" },
            new Skill { Id = 33, Name = "Firebase Firestore", ProficiencyLevel = 4, Category = "Databases" },
            
            // Cloud & DevOps
            new Skill { Id = 34, Name = "Azure", ProficiencyLevel = 3, Category = "Cloud & DevOps" },
            new Skill { Id = 35, Name = "Firebase", ProficiencyLevel = 4, Category = "Cloud & DevOps" },
            new Skill { Id = 36, Name = "CI/CD Pipelines", ProficiencyLevel = 3, Category = "Cloud & DevOps" },
            new Skill { Id = 37, Name = "Git", ProficiencyLevel = 5, Category = "Cloud & DevOps" },
            new Skill { Id = 38, Name = "GitHub", ProficiencyLevel = 5, Category = "Cloud & DevOps" },
            new Skill { Id = 39, Name = "GitLab", ProficiencyLevel = 4, Category = "Cloud & DevOps" },
            
            // Development Tools
            new Skill { Id = 40, Name = "Visual Studio 2022", ProficiencyLevel = 5, Category = "Development Tools" },
            new Skill { Id = 41, Name = "VS Code", ProficiencyLevel = 5, Category = "Development Tools" },
            new Skill { Id = 42, Name = "IntelliJ IDEA", ProficiencyLevel = 3, Category = "Development Tools" },
            new Skill { Id = 43, Name = "Postman", ProficiencyLevel = 4, Category = "Development Tools" },
            new Skill { Id = 44, Name = "phpMyAdmin", ProficiencyLevel = 4, Category = "Development Tools" },
            
            // Design & UI/UX
            new Skill { Id = 45, Name = "Figma", ProficiencyLevel = 4, Category = "Design & UI/UX" },
            new Skill { Id = 46, Name = "Adobe XD", ProficiencyLevel = 3, Category = "Design & UI/UX" },
            
            // Testing & Quality
            new Skill { Id = 47, Name = "Unit Testing", ProficiencyLevel = 4, Category = "Testing & Quality" },
            new Skill { Id = 48, Name = "Debugging", ProficiencyLevel = 5, Category = "Testing & Quality" },
            new Skill { Id = 49, Name = "Code Reviews", ProficiencyLevel = 4, Category = "Testing & Quality" },
            new Skill { Id = 50, Name = "Performance Optimization", ProficiencyLevel = 4, Category = "Testing & Quality" },
            
            // Methodologies
            new Skill { Id = 51, Name = "Agile/Scrum", ProficiencyLevel = 4, Category = "Methodologies" },
            new Skill { Id = 52, Name = "Agile SDLC", ProficiencyLevel = 4, Category = "Methodologies" },
            
            // Emerging Technologies
            new Skill { Id = 53, Name = "Machine Learning", ProficiencyLevel = 3, Category = "Emerging Technologies" },
            new Skill { Id = 54, Name = "LLMs", ProficiencyLevel = 3, Category = "Emerging Technologies" },
            new Skill { Id = 55, Name = "AI Development", ProficiencyLevel = 3, Category = "Emerging Technologies" },
            new Skill { Id = 56, Name = "Automation", ProficiencyLevel = 4, Category = "Emerging Technologies" }
        };

        public IActionResult Index()
        {
            var groupedSkills = _skills.GroupBy(s => s.Category).ToDictionary(g => g.Key, g => g.ToList());
            return View(groupedSkills);
        }
    }
}