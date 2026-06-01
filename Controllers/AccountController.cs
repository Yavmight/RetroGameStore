using Microsoft.AspNetCore.Mvc;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Redirect if already logged in
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _db.Users.FirstOrDefault(u =>
                u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Store session (same pattern as template)
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectBasedOnRole(user.Role.ToString());
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Helper: redirect to the right page based on role
        private IActionResult RedirectBasedOnRole(string role)
        {
            return role switch
            {
                "Admin"    => RedirectToAction("Index", "Dashboard"),
                "Staff"    => RedirectToAction("Index", "Dashboard"),
                "Customer" => RedirectToAction("Browse", "Orders"),
                _          => RedirectToAction("Login")
            };
        }
    }
}
