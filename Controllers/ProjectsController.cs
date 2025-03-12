using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string technology = null, string viewMode = "grid", int page = 1, int pageSize = 4)
        {
            // First fetch all projects to extract technologies
            var allProjects = await _context.Projects.ToListAsync();

            // Get all unique technologies for the filter dropdown
            var allTechnologies = allProjects
                .SelectMany(p => p.Technologies)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            IEnumerable<Project> filteredProjects;

            // Filter projects by technology if selected
            if (!string.IsNullOrEmpty(technology))
            {
                filteredProjects = allProjects
                    .Where(p => p.Technologies.Contains(technology))
                    .ToList();
            }
            else
            {
                filteredProjects = allProjects;
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

        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}