using System.ComponentModel.DataAnnotations;

namespace RetroGameStore.Models
{
    public class CheckoutReviewViewModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Receive Order By")]
        public string FulfillmentType { get; set; } = "Store Pickup";

        [Display(Name = "Delivery Address")]
        public string? DeliveryAddress { get; set; }

        public string PaymentMethod { get; set; } = "Cash";

        public decimal Total => CartItems.Sum(c => c.Price * c.Quantity);
        public int TotalItems => CartItems.Sum(c => c.Quantity);
    }
}
