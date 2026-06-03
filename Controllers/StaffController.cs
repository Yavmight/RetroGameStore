using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _db;

        public StaffController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Staff/Index — Admin only
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var staffList = _db.Users
                .Where(u => u.Role == UserRole.Staff)
                .OrderBy(u => u.FullName)
                .ToList();

            ViewBag.UserRole = "Admin";
            return View(staffList);
        }

        // GET: /Staff/Create — Admin only
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.UserRole = "Admin";
            return View();
        }

        // POST: /Staff/Create — Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.UserRole = "Admin";

            if (!ModelState.IsValid) return View(model);

            var email = model.Email.Trim().ToLower();
            if (_db.Users.Any(u => u.Email.ToLower() == email))
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            var user = new User
            {
                FullName = $"{model.Name.Trim()} {model.Surname.Trim()}",
                Email = email,
                Password = model.Password,
                Role = UserRole.Staff
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["Success"] = $"Staff member '{user.FullName}' registered successfully.";
            return RedirectToAction("Index");
        }

        // POST: /Staff/Delete/3 — Admin only
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var staff = _db.Users.Find(id);
            if (staff == null) return NotFound();

            if (staff.Role != UserRole.Staff)
                return BadRequest("Can only delete staff members from here.");

            _db.Users.Remove(staff);
            _db.SaveChanges();

            TempData["Success"] = $"Staff member '{staff.FullName}' removed.";
            return RedirectToAction("Index");
        }
    }
}
