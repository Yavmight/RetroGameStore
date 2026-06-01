using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;

        public CustomersController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Customers/Index — Admin only
        public IActionResult Index(string? search)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var customers = _db.Users
                .Where(u => u.Role == UserRole.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                customers = customers.Where(u =>
                    u.FullName.Contains(search) || u.Email.Contains(search));

            // Order count per customer
            var orderCounts = _db.Orders
                .GroupBy(o => o.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewBag.OrderCounts = orderCounts;
            ViewBag.Search      = search;
            ViewBag.UserRole    = "Admin";

            return View(customers.OrderBy(u => u.FullName).ToList());
        }

        // GET: /Customers/Details/3 — Admin only
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var customer = _db.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.Game)
                .FirstOrDefault(u => u.Id == id && u.Role == UserRole.Customer);

            if (customer == null) return NotFound();

            ViewBag.UserRole = "Admin";
            return View(customer);
        }

        // POST: /Customers/Delete/3 — Admin only
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            var customer = _db.Users.Find(id);
            if (customer == null) return NotFound();

            _db.Users.Remove(customer);
            _db.SaveChanges();

            TempData["Success"] = $"Customer '{customer.FullName}' removed.";
            return RedirectToAction("Index");
        }
    }
}
