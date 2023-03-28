using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product;

public partial class ProductController
{
    [HttpPut("AddStock")]
    [Authorize(Policy = "Stock_Admin")]
    public async Task<IActionResult> AddStock([FromBody] AddStockCommand request) => Ok(await _mediator.Send(request));
}

public class AddStockCommand : IRequest<AddStockCommandResponse>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddStockCommandResponse
{
    public bool Success { get; set; }
    public List<Notification> Notifications { get; set; }
}

public class AddStockCommandHandler : IRequestHandler<AddStockCommand, AddStockCommandResponse>
{
    private readonly ProductDbContext _productDbContext;

    public AddStockCommandHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<AddStockCommandResponse> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        var response = new AddStockCommandResponse() { Notifications = new List<Notification>() };

        // Fail fast validations
        if (request.Quantity <= 0)
            response.Notifications.Add(new Notification() { Message = "Quantity must be greater than zero", Type = ENotificationType.Error });

        if (request.ProductId == Guid.Empty)
            response.Notifications.Add(new Notification() { Message = "Invalid productId", Type = ENotificationType.Error });

        if (response.Notifications.Any(c => c.Type == ENotificationType.Error))
            return response;

        var productFromDb = await _productDbContext.Product.FirstOrDefaultAsync(Product => Product.Id == request.ProductId);
        if (productFromDb is null)
            return new AddStockCommandResponse()
            {
                Success = false,
                Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = "Product does not exist",
                            Type = ENotificationType.Error
                        }
                    }
            };

        productFromDb.AddStock(request.Quantity);

        await _productDbContext.SaveChangesAsync();

        return new AddStockCommandResponse()
        {
            Success = true
        };
    }
}
