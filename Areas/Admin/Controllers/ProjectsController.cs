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
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment,
            ILogger<ProjectsController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: Admin/Projects
        public async Task<IActionResult> Index()
        {
            try
            {
                var projects = await _context.Projects.ToListAsync();
                return View(projects.OrderByDescending(p => p.CompletionDate).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects index");
                TempData["ErrorMessage"] = "Error loading projects: " + ex.Message;
                return View(new List<Project>());
            }
        }

        // GET: Admin/Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (project == null)
                {
                    return NotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading project details");
                TempData["ErrorMessage"] = "Error loading project details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
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
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Creating new project: {Title}", project.Title);

                    // Handle Technologies (comma-separated string)
                    if (!string.IsNullOrEmpty(technologies))
                    {
                        project.Technologies = technologies.Split(',')
                            .Select(t => t.Trim())
                            .Where(t => !string.IsNullOrEmpty(t))
                            .ToList();

                        _logger.LogInformation("Technologies: {Technologies}",
                            string.Join(", ", project.Technologies));
                    }
                    else
                    {
                        project.Technologies = new List<string>();
                    }

                    // Handle Image Upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        _logger.LogInformation("Processing image upload: {FileName}", imageFile.FileName);

                        try
                        {
                            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "projects");

                            // Ensure directory exists
                            if (!Directory.Exists(uploadsFolder))
                            {
                                _logger.LogInformation("Creating directory: {Path}", uploadsFolder);
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            _logger.LogInformation("Saving image to: {Path}", filePath);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(fileStream);
                            }

                            project.ImageUrl = $"/images/projects/{uniqueFileName}";
                            _logger.LogInformation("Image saved, URL set to: {Url}", project.ImageUrl);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error saving image");
                            project.ImageUrl = "/images/projects/default-project.jpg";
                        }
                    }
                    else
                    {
                        // Default image if none provided
                        project.ImageUrl = "/images/projects/default-project.jpg";
                    }

                    _context.Add(project);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Project created successfully: {Id}", project.Id);
                    TempData["SuccessMessage"] = "Project created successfully!";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                ModelState.AddModelError("", "An error occurred while saving the project: " + ex.Message);
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

            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                // Convert technologies to comma-separated string for the form
                ViewBag.Technologies = string.Join(",", project.Technologies ?? new List<string>());

                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading project for edit");
                TempData["ErrorMessage"] = "Error loading project for editing: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
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

            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Updating project: {Id}", id);

                    // Get existing project to preserve data
                    var existingProject = await _context.Projects
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

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
                        _logger.LogInformation("Processing updated image: {FileName}", imageFile.FileName);

                        try
                        {
                            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "projects");
                            // Ensure directory exists
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(fileStream);
                            }

                            project.ImageUrl = $"/images/projects/{uniqueFileName}";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error updating image");
                            project.ImageUrl = existingProject.ImageUrl; // Keep existing image on error
                        }
                    }
                    else
                    {
                        // Keep existing image
                        project.ImageUrl = existingProject.ImageUrl;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Project updated successfully");
                    TempData["SuccessMessage"] = "Project updated successfully!";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProjectExists(project.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating project");
                    ModelState.AddModelError("", "This project was updated by another user. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project");
                ModelState.AddModelError("", "An error occurred while updating the project: " + ex.Message);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Technologies = technologies;
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (project == null)
                {
                    return NotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading project for deletion");
                TempData["ErrorMessage"] = "Error loading project for deletion: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);

                if (project == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Deleting project: {Id}", id);

                // Delete project image if it exists and isn't the default
                if (!string.IsNullOrEmpty(project.ImageUrl) &&
                    !project.ImageUrl.Contains("default-project.jpg"))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, project.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                            _logger.LogInformation("Deleted project image: {Path}", imagePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to delete project image");
                            // Continue with project deletion even if image deletion fails
                        }
                    }
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project deleted successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project");
                TempData["ErrorMessage"] = "Error deleting project: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}