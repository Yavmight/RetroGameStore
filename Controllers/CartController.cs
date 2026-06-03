using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        // Auth helper
        private bool IsLoggedIn() => HttpContext.Session.GetString("UserEmail") != null;

        // Get cart from session
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        // Save cart to session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }

        // GET: /Cart/Index
        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int gameId, int quantity = 1)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var game = _db.Games.Find(gameId);
            if (game == null)
            {
                TempData["Error"] = "Game not found.";
                return RedirectToAction("Browse", "Orders");
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.GameId == gameId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                if (existing.Quantity > game.Stock)
                    existing.Quantity = game.Stock;
            }
            else
            {
                cart.Add(new CartItem
                {
                    GameId = game.Id,
                    Title = game.Title,
                    Price = game.Price,
                    Quantity = Math.Min(quantity, game.Stock),
                    ImageUrl = game.CoverImageUrl,
                    Platform = game.Platform.ToString()
                });
            }

            SaveCart(cart);
            TempData["Success"] = $"'{game.Title}' added to cart!";
            return RedirectToAction("Browse", "Orders");
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart(int gameId)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            cart.RemoveAll(c => c.GameId == gameId);
            SaveCart(cart);
            TempData["Success"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int gameId, int quantity)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.GameId == gameId);
            if (item != null)
            {
                var game = _db.Games.Find(gameId);
                if (game != null)
                {
                    item.Quantity = Math.Max(1, Math.Min(quantity, game.Stock));
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // POST: /Cart/Checkout
        [HttpPost]
        public IActionResult Checkout()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            foreach (var item in cart)
            {
                var game = _db.Games.Find(item.GameId);
                if (game == null || game.Stock < item.Quantity)
                {
                    TempData["Error"] = $"Not enough stock for '{item.Title}'.";
                    return RedirectToAction("Index");
                }

                var order = new Order
                {
                    UserId = userId.Value,
                    GameId = item.GameId,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Completed
                };

                game.Stock -= item.Quantity;
                _db.Orders.Add(order);
            }

            _db.SaveChanges();

            // Clear cart
            HttpContext.Session.Remove(CartSessionKey);

            TempData["Success"] = $"Order placed! {cart.Count} item(s) purchased successfully.";
            return RedirectToAction("MyOrders", "Orders");
        }

        // Helper: get cart count (used via ViewComponent or ViewBag)
        public static int GetCartCount(ISession session)
        {
            var json = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json)) return 0;
            var cart = JsonSerializer.Deserialize<List<CartItem>>(json);
            return cart?.Sum(c => c.Quantity) ?? 0;
        }
    }
}
