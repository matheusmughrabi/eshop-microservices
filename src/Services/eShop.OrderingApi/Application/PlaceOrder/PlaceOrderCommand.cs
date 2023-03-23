using MediatR;
using System.Text.Json.Serialization;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommand : IRequest<PlaceOrderCommandResult>
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
    }
}