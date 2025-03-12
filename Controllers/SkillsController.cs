using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class SkillsController : Controller
    {
        private static List<Skill> _skills = new List<Skill>
    {
        new Skill { Id = 1, Name = "C#", ProficiencyLevel = 5, Category = "Backend" },
        new Skill { Id = 2, Name = "ASP.NET Core", ProficiencyLevel = 4, Category = "Backend" },
        new Skill { Id = 3, Name = "SQL Server", ProficiencyLevel = 4, Category = "Database" },
        new Skill { Id = 4, Name = "JavaScript", ProficiencyLevel = 4, Category = "Frontend" },
        new Skill { Id = 5, Name = "HTML/CSS", ProficiencyLevel = 5, Category = "Frontend" },
        new Skill { Id = 6, Name = "React", ProficiencyLevel = 3, Category = "Frontend" }
        // Add more skills as needed
    };

        public IActionResult Index()
        {
            var groupedSkills = _skills.GroupBy(s => s.Category).ToDictionary(g => g.Key, g => g.ToList());
            return View(groupedSkills);
        }
    }
}
