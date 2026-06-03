namespace RetroGameStore.Models
{
    public enum OrderStatus
    {
        Completed,
        Refunded,
        Pending
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Completed;
        public string PaymentMethod { get; set; } = "Cash";
        public string FulfillmentType { get; set; } = "Store Pickup";
        public string? DeliveryAddress { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
