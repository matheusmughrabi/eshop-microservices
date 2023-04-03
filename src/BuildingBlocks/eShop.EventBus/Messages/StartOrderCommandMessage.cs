using eShop.EventBus.Base;

namespace eShop.EventBus.Messages;

public class StartOrderCommandMessage : IEventMessage
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
