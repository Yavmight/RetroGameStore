using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Auth guard — only Admin and Staff
            var role = HttpContext.Session.GetString("UserRole");
            if (role == null) return RedirectToAction("Login", "Account");
            if (role == "Customer") return RedirectToAction("Browse", "Orders");

            // Stats for the dashboard cards
            ViewBag.TotalGames      = _db.Games.Count();
            ViewBag.TotalStock      = _db.Games.Sum(g => g.Stock);
            ViewBag.LowStockCount   = _db.Games.Count(g => g.Stock <= g.LowStockThreshold);
            ViewBag.TotalSales      = _db.Orders.Count();
            ViewBag.TotalRevenue    = _db.Orders
                                        .Where(o => o.Status == OrderStatus.Completed)
                                        .AsEnumerable()
                                        .Sum(o => o.TotalPrice);
            ViewBag.TotalCustomers  = _db.Users.Count(u => u.Role == UserRole.Customer);

            // Recent 5 orders for the activity table
            var recentOrders = _db.Orders
                .Include(o => o.User)
                .Include(o => o.Game)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = role;

            return View(recentOrders);
        }
    }
}
