using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            return RedirectToAction("LoginCustomer");
        }

        [HttpGet]
        public IActionResult LoginCustomer()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Customer";
            return View("Login");
        }

        [HttpGet]
        public IActionResult LoginStaff()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Staff";
            return View("LoginStaff");
        }

        [HttpGet]
        public IActionResult LoginAdmin()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            ViewBag.LoginType = "Admin";
            return View("LoginAdmin");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model, string? loginType)
        {
            if (!ModelState.IsValid)
                return LoginViewFor(loginType, model);

            var user = _db.Users.FirstOrDefault(u =>
                u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return LoginViewFor(loginType, model);
            }

            if (!string.IsNullOrEmpty(loginType) && user.Role.ToString() != loginType)
            {
                ModelState.AddModelError("", $"This account is not a {loginType} account.");
                return LoginViewFor(loginType, model);
            }

            SignIn(user);
            return RedirectBasedOnRole(user.Role.ToString());
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole")!);

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
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
                Role = UserRole.Customer
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            SignIn(user);

            TempData["Success"] = "Welcome! Your customer account has been created.";
            return RedirectToAction("Browse", "Orders");
        }

        public IActionResult CustomerProfile()
        {
            if (HttpContext.Session.GetString("UserRole") != "Customer")
                return RedirectToAction("LoginCustomer");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LoginCustomer");

            var user = _db.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null) return RedirectToAction("Logout");

            var orders = _db.Orders
                .Include(o => o.Game)
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(new ProfileViewModel { User = user, Orders = orders });
        }

        public IActionResult StaffProfile()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Staff" && role != "Admin")
                return RedirectToAction("LoginStaff");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserRole = role;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginCustomer");
        }

        private void SignIn(User user)
        {
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetInt32("UserId", user.Id);
        }

        private IActionResult LoginViewFor(string? loginType, LoginViewModel model)
        {
            return loginType switch
            {
                "Staff" => View("LoginStaff", model),
                "Admin" => View("LoginAdmin", model),
                _ => View("Login", model)
            };
        }

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
