using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product;

public class SubtractFromStockCommand : IRequest<SubtractFromStockCommandResponse>
{
    public string OrderId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
public class SubtractFromStockCommandResponse
{
    public bool Success { get; set; }
    public List<Notification> Notifications { get; set; }
}

public class SubtractFromStockCommandHandler : IRequestHandler<SubtractFromStockCommand, SubtractFromStockCommandResponse>
{
    private readonly ProductDbContext _productDbContext;

    public SubtractFromStockCommandHandler(
        ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<SubtractFromStockCommandResponse> Handle(SubtractFromStockCommand request, CancellationToken cancellationToken)
    {
        var productsFromDb = await _productDbContext.Product
            .Where(product => request.Products.Select(c => c.Id).Contains(product.Id))
            .ToListAsync();

        var productsWithNotEnoughQuantity = new List<Guid>();
        foreach (var product in productsFromDb)
        {
            var quantity = request.Products.FirstOrDefault(c => c.Id == product.Id).Quantity;
            product.SubtractStock(quantity);
        }

        if (productsWithNotEnoughQuantity.Count == 0)
            await _productDbContext.SaveChangesAsync();

        return new SubtractFromStockCommandResponse()
        {
            Success = true
        };
    }
}