namespace RetroGameStore.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; } = null!;
        public List<Order> Orders { get; set; } = new();
    }
}
