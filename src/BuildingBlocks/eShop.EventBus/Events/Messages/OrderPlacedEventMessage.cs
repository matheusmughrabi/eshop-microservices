using eShop.EventBus.Events.Base;

namespace eShop.EventBus.Events.Messages;

public class OrderPlacedEventMessage : IEventMessage
{
    public string OrderId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
