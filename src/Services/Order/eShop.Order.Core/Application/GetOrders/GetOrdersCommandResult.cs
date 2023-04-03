using eShop.Order.Core.Domain.Enums;

namespace eShop.Order.Core.Application.GetOrders;

public class GetOrdersCommandResult
{
    public List<Order> Orders { get; set; }

    public class Order
    {
        public string Id { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string StatusDescription { get; set; }
        public List<Product> Products { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }

    public class Notification
    {
        public string Description { get; set; }
    }
}

