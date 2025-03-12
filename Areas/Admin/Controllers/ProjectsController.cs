using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects.OrderByDescending(p => p.CompletionDate).ToList());
        }

        // GET: Admin/Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Admin/Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ProjectUrl,GitHubUrl,CompletionDate,IsFeatured")] Project project, IFormFile imageFile, string technologies)
        {
            if (ModelState.IsValid)
            {
                // Handle Technologies (comma-separated string)
                if (!string.IsNullOrEmpty(technologies))
                {
                    project.Technologies = technologies.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList();
                }
                else
                {
                    project.Technologies = new List<string>();
                }

                // Handle Image Upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "projects");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    project.ImageUrl = $"/images/projects/{uniqueFileName}";
                }
                else
                {
                    // Default image if none provided
                    project.ImageUrl = "/images/projects/default-project.jpg";
                }

                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Admin/Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.Technologies = string.Join(",", project.Technologies);
            return View(project);
        }

        // POST: Admin/Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ProjectUrl,GitHubUrl,CompletionDate,IsFeatured")] Project project, IFormFile imageFile, string technologies)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get existing project to preserve image if no new one uploaded
                    var existingProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

                    // Handle Technologies (comma-separated string)
                    if (!string.IsNullOrEmpty(technologies))
                    {
                        project.Technologies = technologies.Split(',')
                            .Select(t => t.Trim())
                            .Where(t => !string.IsNullOrEmpty(t))
                            .ToList();
                    }
                    else
                    {
                        project.Technologies = new List<string>();
                    }

                    // Handle Image Upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "projects");
                        Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Delete old image if it exists and isn't the default
                        if (!string.IsNullOrEmpty(existingProject.ImageUrl) &&
                            !existingProject.ImageUrl.Contains("default-project.jpg") &&
                            System.IO.File.Exists(Path.Combine(_hostEnvironment.WebRootPath, existingProject.ImageUrl.TrimStart('/'))))
                        {
                            System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath, existingProject.ImageUrl.TrimStart('/')));
                        }

                        project.ImageUrl = $"/images/projects/{uniqueFileName}";
                    }
                    else
                    {
                        // Keep existing image
                        project.ImageUrl = existingProject.ImageUrl;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Project updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            // Delete project image if it exists and isn't the default
            if (project != null && !string.IsNullOrEmpty(project.ImageUrl) &&
                !project.ImageUrl.Contains("default-project.jpg") &&
                System.IO.File.Exists(Path.Combine(_hostEnvironment.WebRootPath, project.ImageUrl.TrimStart('/'))))
            {
                System.IO.File.Delete(Path.Combine(_hostEnvironment.WebRootPath, project.ImageUrl.TrimStart('/')));
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Project deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}