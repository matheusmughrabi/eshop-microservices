using eShop.EventBus.Messages;
using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Domain;
using eShop.ProductApi.Events.Publishers;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product;

public class CheckStockCommand : IRequest<CheckStockCommandResponse>
{
    public string OrderId { get; set; }
    public List<Product> Products { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
public class CheckStockCommandResponse
{
    public bool Success { get; set; }
    public List<Notification> Notifications { get; set; }
}

public class CheckStockCommandHandler : IRequestHandler<CheckStockCommand, CheckStockCommandResponse>
{
    private readonly ProductDbContext _productDbContext;
    private readonly StockValidatedEventPublisher _stockValidatedPublisher;

    public CheckStockCommandHandler(
        ProductDbContext productDbContext, 
        StockValidatedEventPublisher stockValidatedPublisher)
    {
        _productDbContext = productDbContext;
        _stockValidatedPublisher = stockValidatedPublisher;
    }

    public async Task<CheckStockCommandResponse> Handle(CheckStockCommand request, CancellationToken cancellationToken)
    {
        var response = new CheckStockCommandResponse() { Notifications = new List<Notification>() };

        // Fail fast validations
        if (request.Products.Any(product => product.Quantity <= 0))
            response.Notifications.Add(new Notification() { Message = "Quantity must be greater than zero", Type = ENotificationType.Error });

        if (request.Products.Any(product => product.Id == Guid.Empty))
            response.Notifications.Add(new Notification() { Message = "Invalid productId", Type = ENotificationType.Error });

        if (response.Notifications.Any(c => c.Type == ENotificationType.Error))
            return response;

        var productsFromDb = await _productDbContext.Product
            .AsNoTracking()
            .Where(product => request.Products.Select(c => c.Id).Contains(product.Id))
            .ToListAsync();

        var productsWithNotEnoughQuantity = new List<ProductEntity>();
        foreach (var product in productsFromDb)
        {
            var quantity = request.Products.FirstOrDefault(c => c.Id == product.Id).Quantity;

            if (product.QuantityOnHand < quantity)
            {
                productsWithNotEnoughQuantity.Add(product);
                continue;
            }
        }

        _stockValidatedPublisher.Publish(new EventBus.Messages.StockValidatedEventMessage()
        {
            Success = productsWithNotEnoughQuantity.Count == 0,
            OrderId = request.OrderId,
            UnderstockedProducts = productsWithNotEnoughQuantity.Select(product => new StockValidatedEventMessage.Product()
            {
                Id = product.Id,
                Name = product.Name
            }).ToList()
        });

        return new CheckStockCommandResponse()
        {
            Success = true
        };
    }
}
