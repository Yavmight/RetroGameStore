namespace RetroGameStore.Models
{
    public enum Platform
    {
        PlayStation1,
        PlayStation2,
        GameBoy,
        GBA,
        Nintendo64,
        GameCube
    }

    public enum Genre
    {
        Action,
        Adventure,
        RPG,
        Sports,
        Racing,
        Fighting,
        Platformer,
        Shooter,
        Puzzle,
        Strategy
    }

    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Platform Platform { get; set; }
        public Genre Genre { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int LowStockThreshold { get; set; } = 3;
        public string? CoverImageUrl { get; set; }
        public int ReleaseYear { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // Computed helper
        public bool IsLowStock => Stock <= LowStockThreshold;
    }
}
