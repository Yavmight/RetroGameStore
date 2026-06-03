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

        // GET: /Account/Login — redirects to LoginCustomer
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            return RedirectToAction("LoginCustomer");
        }

        // GET: /Account/LoginCustomer
        [HttpGet]
        public IActionResult LoginCustomer()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Customer";
            return View("Login");
        }

        // GET: /Account/LoginStaff
        [HttpGet]
        public IActionResult LoginStaff()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Staff";
            return View("LoginStaff");
        }

        // GET: /Account/LoginAdmin
        [HttpGet]
        public IActionResult LoginAdmin()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Admin";
            return View("LoginAdmin");
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string? loginType)
        {
            if (!ModelState.IsValid)
            {
                return loginType switch
                {
                    "Staff" => View("LoginStaff", model),
                    "Admin" => View("LoginAdmin", model),
                    _ => View("Login", model)
                };
            }

            var user = _db.Users.FirstOrDefault(u =>
                u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return loginType switch
                {
                    "Staff" => View("LoginStaff", model),
                    "Admin" => View("LoginAdmin", model),
                    _ => View("Login", model)
                };
            }

            // Validate role match
            if (!string.IsNullOrEmpty(loginType) && user.Role.ToString() != loginType)
            {
                ModelState.AddModelError("", $"This account is not a {loginType} account.");
                return loginType switch
                {
                    "Staff" => View("LoginStaff", model),
                    "Admin" => View("LoginAdmin", model),
                    _ => View("Login", model)
                };
            }

            // Store session
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
            return RedirectToAction("LoginCustomer");
        }

        // Helper: redirect to the right page based on role
        private IActionResult RedirectBasedOnRole(string role)
        {
            return role switch
            {
                "Admin"    => RedirectToAction("Index", "Dashboard"),
                "Staff"    => RedirectToAction("Index", "Dashboard"),
                "Customer" => RedirectToAction("Browse", "Orders"),
                _          => RedirectToAction("LoginCustomer")
            };
        }
    }
}
