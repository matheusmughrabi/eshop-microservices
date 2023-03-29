using eShop.EventBus.Events.Base;

namespace eShop.EventBus.Events.BasketCheckout;

public class BasketCheckoutEventMessage : IEventMessage
{
    public string? UserId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
        public string? ImagePath { get; set; }
    }
}
