using Microsoft.AspNetCore.Mvc;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.Now;
                _context.Add(contact);
                await _context.SaveChangesAsync();

                // For now, we'll just show a success message
                TempData["Message"] = "Thank you for your message! I'll get back to you soon.";
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }
    }
}