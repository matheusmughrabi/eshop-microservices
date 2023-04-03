using eShop.EventBus.Base;

namespace eShop.EventBus.Messages;

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
