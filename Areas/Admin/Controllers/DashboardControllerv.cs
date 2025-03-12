using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var projectCount = await _context.Projects.CountAsync();
            var skillCount = await _context.Skills.CountAsync();
            var contactCount = await _context.Contacts.CountAsync();
            var featuredProjectCount = await _context.Projects.Where(p => p.IsFeatured).CountAsync();

            ViewBag.ProjectCount = projectCount;
            ViewBag.SkillCount = skillCount;
            ViewBag.ContactCount = contactCount;
            ViewBag.FeaturedProjectCount = featuredProjectCount;

            var recentProjects = await _context.Projects
                .OrderByDescending(p => p.CompletionDate)
                .Take(5)
                .ToListAsync();

            var recentContacts = await _context.Contacts
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentProjects = recentProjects;
            ViewBag.RecentContacts = recentContacts;

            return View();
        }
    }
}