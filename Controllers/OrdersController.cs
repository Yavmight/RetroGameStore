using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        // Auth helpers
        private bool IsLoggedIn() => HttpContext.Session.GetString("UserEmail") != null;
        private bool IsAdminOrStaff()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" || role == "Staff";
        }

        // GET: /Orders/SalesLog — Admin & Staff only
        public IActionResult SalesLog(string? status)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Login", "Account");

            var orders = _db.Orders
                .Include(o => o.User)
                .Include(o => o.Game)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, out var s))
                orders = orders.Where(o => o.Status == s);

            ViewBag.StatusFilter = status;
            ViewBag.UserRole     = HttpContext.Session.GetString("UserRole");

            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        // POST: /Orders/UpdateStatus — Admin only
        [HttpPost]
        public IActionResult UpdateStatus(int id, OrderStatus status)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("SalesLog");

            var order = _db.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _db.SaveChanges();
            TempData["Success"] = "Order status updated.";
            return RedirectToAction("SalesLog");
        }

        // GET: /Orders/Browse — Customers (and everyone) can see this
        public IActionResult Browse(string? search, string? platform)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var games = _db.Games.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                games = games.Where(g => g.Title.Contains(search));

            if (!string.IsNullOrEmpty(platform) && Enum.TryParse<Platform>(platform, out var p))
                games = games.Where(g => g.Platform == p);

            ViewBag.Search   = search;
            ViewBag.Platform = platform;
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(games.Where(g => g.Stock > 0).OrderBy(g => g.Title).ToList());
        }

        // POST: /Orders/Buy — Customer purchases a game
        [HttpPost]
        public IActionResult Buy(int gameId, int quantity)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var game = _db.Games.Find(gameId);
            if (game == null || game.Stock < quantity)
            {
                TempData["Error"] = "Not enough stock available.";
                return RedirectToAction("Browse");
            }

            var order = new Order
            {
                UserId     = userId.Value,
                GameId     = gameId,
                Quantity   = quantity,
                TotalPrice = game.Price * quantity,
                OrderDate  = DateTime.Now,
                Status     = OrderStatus.Completed
            };

            game.Stock -= quantity;

            _db.Orders.Add(order);
            _db.SaveChanges();

            TempData["Success"] = $"You bought {quantity}x '{game.Title}' for ${order.TotalPrice:F2}!";
            return RedirectToAction("MyOrders");
        }

        // GET: /Orders/MyOrders — Customer's personal order history
        public IActionResult MyOrders()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId");

            var orders = _db.Orders
                .Include(o => o.Game)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            return View(orders);
        }
    }
}
