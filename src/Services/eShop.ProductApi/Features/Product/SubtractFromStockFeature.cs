using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Events.Publishers;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product;

public class SubtractFromStockCommand : IRequest<SubtractStockCommandResponse>
{
    public string OrderId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
public class SubtractStockCommandResponse
{
    public bool Success { get; set; }
    public List<Notification> Notifications { get; set; }
}

public class SubtractStockCommandHandler : IRequestHandler<SubtractFromStockCommand, SubtractStockCommandResponse>
{
    private readonly ProductDbContext _productDbContext;
    private readonly ProductsSubtractedFromStockEventPublisher _productsSubtractedPublisher;

    public SubtractStockCommandHandler(
        ProductDbContext productDbContext, 
        ProductsSubtractedFromStockEventPublisher productsSubtractedPublisher)
    {
        _productDbContext = productDbContext;
        _productsSubtractedPublisher = productsSubtractedPublisher;
    }

    public async Task<SubtractStockCommandResponse> Handle(SubtractFromStockCommand request, CancellationToken cancellationToken)
    {
        var response = new SubtractStockCommandResponse() { Notifications = new List<Notification>() };

        // Fail fast validations
        if (request.Products.Any(product => product.Quantity <= 0))
            response.Notifications.Add(new Notification() { Message = "Quantity must be greater than zero", Type = ENotificationType.Error });

        if (request.Products.Any(product => product.Id == Guid.Empty))
            response.Notifications.Add(new Notification() { Message = "Invalid productId", Type = ENotificationType.Error });

        if (response.Notifications.Any(c => c.Type == ENotificationType.Error))
            return response;

        var productsFromDb = await _productDbContext.Product
            .Where(product => request.Products.Select(c => c.Id).Contains(product.Id))
            .ToListAsync();

        var productsWithNotEnoughQuantity = new List<Guid>();
        foreach (var product in productsFromDb)
        {
            var quantity = request.Products.FirstOrDefault(c => c.Id == product.Id).Quantity;

            if (product.QuantityOnHand < quantity)
            {
                productsWithNotEnoughQuantity.Add(product.Id);
                continue;
            }

            product.SubtractStock(quantity);
        }

        if (productsWithNotEnoughQuantity.Count == 0)
            await _productDbContext.SaveChangesAsync();

        _productsSubtractedPublisher.Publish(new EventBus.Events.Messages.ProductsSubtractedFromStockEventMessage()
        {
            Success = productsWithNotEnoughQuantity.Count == 0,
            OrderId = request.OrderId,
            UnderstockedProducts = productsWithNotEnoughQuantity.Select(id => new EventBus.Events.Messages.ProductsSubtractedFromStockEventMessage.Product()
            {
                Id = id
            }).ToList()
        });

        return new SubtractStockCommandResponse()
        {
            Success = true
        };
    }
}
