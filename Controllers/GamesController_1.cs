using Microsoft.AspNetCore.Mvc;
using RetroGameStore.Models;

namespace RetroGameStore.Controllers
{
    public class GamesController : Controller
    {
        private readonly AppDbContext _db;

        public GamesController(AppDbContext db)
        {
            _db = db;
        }

        // Auth helper
        private bool IsAdminOrStaff()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" || role == "Staff";
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: /Games/Index
        public IActionResult Index(string? search, string? platform, string? genre)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Login", "Account");

            var games = _db.Games.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                games = games.Where(g => g.Title.Contains(search));

            if (!string.IsNullOrEmpty(platform) && Enum.TryParse<Platform>(platform, out var p))
                games = games.Where(g => g.Platform == p);

            if (!string.IsNullOrEmpty(genre) && Enum.TryParse<Genre>(genre, out var g2))
                games = games.Where(g => g.Genre == g2);

            ViewBag.Search   = search;
            ViewBag.Platform = platform;
            ViewBag.Genre    = genre;
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            return View(games.OrderBy(g => g.Title).ToList());
        }

        // GET: /Games/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index");
            return View();
        }

        // POST: /Games/Create
        [HttpPost]
        public IActionResult Create(Game game)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View(game);

            _db.Games.Add(game);
            _db.SaveChanges();
            TempData["Success"] = $"'{game.Title}' added successfully!";
            return RedirectToAction("Index");
        }

        // GET: /Games/Edit/5
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            var game = _db.Games.Find(id);
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Games/Edit/5
        [HttpPost]
        public IActionResult Edit(Game game)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View(game);

            _db.Games.Update(game);
            _db.SaveChanges();
            TempData["Success"] = $"'{game.Title}' updated successfully!";
            return RedirectToAction("Index");
        }

        // POST: /Games/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index");

            var game = _db.Games.Find(id);
            if (game == null) return NotFound();

            _db.Games.Remove(game);
            _db.SaveChanges();
            TempData["Success"] = $"'{game.Title}' deleted.";
            return RedirectToAction("Index");
        }

        // GET: /Games/LowStock
        public IActionResult LowStock()
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Login", "Account");

            var games = _db.Games
                .Where(g => g.Stock <= g.LowStockThreshold)
                .OrderBy(g => g.Stock)
                .ToList();

            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(games);
        }

        // GET: /Games/Details/5 — accessible to all logged-in users
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
                return RedirectToAction("Login", "Account");

            var game = _db.Games.Find(id);
            if (game == null) return NotFound();

            // Build gallery images (use the cover image + placeholder variations)
            var gallery = new List<string>();
            if (!string.IsNullOrEmpty(game.CoverImageUrl))
            {
                gallery.Add(game.CoverImageUrl);
            }

            // Get related games (same platform or genre, excluding current)
            var relatedGames = _db.Games
                .Where(g => g.Id != game.Id && (g.Platform == game.Platform || g.Genre == game.Genre))
                .Take(4)
                .ToList();

            var viewModel = new GameDetailViewModel
            {
                Game = game,
                GalleryImages = gallery,
                RelatedGames = relatedGames
            };

            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(viewModel);
        }
    }
}
