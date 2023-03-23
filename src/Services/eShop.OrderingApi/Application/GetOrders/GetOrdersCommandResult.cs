using eShop.OrderingApi.Domain.Enums;

namespace eShop.OrderingApi.Application.GetOrders;

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
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
    }
}

