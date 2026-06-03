namespace RetroGameStore.Models
{
    public class GameDetailViewModel
    {
        public Game Game { get; set; } = null!;
        public List<string> GalleryImages { get; set; } = new();
        public List<Game> RelatedGames { get; set; } = new();
    }
}
