using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // You can pass your basic info as ViewData
            ViewData["Name"] = "MANWATHA WANGA COMFORT"; 
            ViewData["Title"] = "Junior .NET Developer";
            ViewData["Summary"] = "I build high-quality web and mobile applications With a focus on clean code and user experience, I deliver solutions that make a difference.";

            // Get featured projects for the homepage
            var projects = await _context.Projects.ToListAsync();
            var featuredProjects = projects
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.CompletionDate)
                .Take(2)
                .ToList();

            ViewBag.FeaturedProjects = featuredProjects;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}