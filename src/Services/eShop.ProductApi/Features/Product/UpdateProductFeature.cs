using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CreateProductCommand request) => Ok(await _mediator.Send(request));
    }

    public class UpdateProductCommand : IRequest<UpdateProductCommandResponse>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public UpdateProductCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productFromDb = await _productDbContext.Product.FirstOrDefaultAsync(Product => Product.Id == request.ProductId);
            if (productFromDb is null)
                return new UpdateProductCommandResponse()
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

            productFromDb.Name = request.Name;
            productFromDb.Description = request.Description;
            productFromDb.Price = request.Price;

            await _productDbContext.SaveChangesAsync();

            return new UpdateProductCommandResponse()
            {
                Success = true
            };
        }
    }
}
