using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand request) => Ok(await _mediator.Send(request));
    }

    public class DeleteProductCommand : IRequest<DeleteProductCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public DeleteProductCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productFromDb = await _productDbContext.Product.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id);
            if (productFromDb == null)
                return new DeleteProductCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification() { Message = "Product not found", Type = ENotificationType.Error }
                    }
                };

            _productDbContext.Remove(productFromDb);
            await _productDbContext.SaveChangesAsync();

            return new DeleteProductCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                {
                    new Notification() { Message = $"Product {productFromDb.Name} deleted.", Type = ENotificationType.Informative }
                }
            };
        }
    }
}
