using eShop.EventBus.Events.Base;

namespace eShop.EventBus.Events.Messages;

public class StockValidatedEventMessage : IEventMessage
{
    public bool Success { get; set; }
    public string OrderId { get; set; }
    public List<Product> UnderstockedProducts { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
