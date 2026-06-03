using Microsoft.EntityFrameworkCore;

namespace RetroGameStore.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Admin User",    Email = "admin@retrogames.com",    Password = "admin123",    Role = UserRole.Admin    },
                new User { Id = 2, FullName = "Staff Member",  Email = "staff@retrogames.com",    Password = "staff123",    Role = UserRole.Staff    },
                new User { Id = 3, FullName = "John Customer", Email = "customer@retrogames.com", Password = "customer123", Role = UserRole.Customer }
            );

            // Seed Games
            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Title = "Crash Bandicoot",        Platform = Platform.PlayStation1, Genre = Genre.Platformer, Price = 29.99m, Stock = 5,  ReleaseYear = 1996, CoverImageUrl = "/images/games/crash_bandicoot.png" },
                new Game { Id = 2, Title = "Gran Turismo 3",         Platform = Platform.PlayStation2, Genre = Genre.Racing,     Price = 24.99m, Stock = 2,  ReleaseYear = 2001, CoverImageUrl = "/images/games/gran_turismo_3.png" },
                new Game { Id = 3, Title = "Pokémon Red",            Platform = Platform.GameBoy,      Genre = Genre.RPG,        Price = 34.99m, Stock = 8,  ReleaseYear = 1996, CoverImageUrl = "/images/games/pokemon_red.png" },
                new Game { Id = 4, Title = "The Legend of Zelda: Minish Cap", Platform = Platform.GBA, Genre = Genre.Adventure, Price = 39.99m, Stock = 3,  ReleaseYear = 2004, CoverImageUrl = "/images/games/zelda_minish_cap.png" },
                new Game { Id = 5, Title = "Super Mario 64",         Platform = Platform.Nintendo64,   Genre = Genre.Platformer, Price = 44.99m, Stock = 1,  ReleaseYear = 1996, CoverImageUrl = "/images/games/super_mario_64.png" },
                new Game { Id = 6, Title = "The Legend of Zelda: Ocarina of Time", Platform = Platform.Nintendo64, Genre = Genre.Adventure, Price = 49.99m, Stock = 2, ReleaseYear = 1998, CoverImageUrl = "/images/games/zelda_ocarina.png" },
                new Game { Id = 7, Title = "Super Smash Bros. Melee",Platform = Platform.GameCube,    Genre = Genre.Fighting,   Price = 54.99m, Stock = 4,  ReleaseYear = 2001, CoverImageUrl = "/images/games/smash_bros_melee.png" },
                new Game { Id = 8, Title = "Resident Evil",          Platform = Platform.PlayStation1, Genre = Genre.Action,    Price = 19.99m, Stock = 6,  ReleaseYear = 1996, CoverImageUrl = "/images/games/resident_evil.png" }
            );
        }
    }
}
