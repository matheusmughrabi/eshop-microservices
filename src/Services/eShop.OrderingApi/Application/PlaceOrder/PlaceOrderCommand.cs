using MediatR;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommand : IRequest<PlaceOrderCommandResponse>
{
    public string UserId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public string Id { get; set; }
        public decimal PriceAtTimeOfOrder { get; set; }
        public int Quantity { get; set; }
    }
}