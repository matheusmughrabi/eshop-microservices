using MediatR;

namespace eShop.Order.Core.Application.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderCommandResult>
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