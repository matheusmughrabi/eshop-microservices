using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand request) => Ok(await _mediator.Send(request));
    }
    public class DeleteCategoryCommand : IRequest<DeleteCategoryCommandResponse>
    {
        public Guid Id { get; set; }
        public bool DeleteProducts { get; set; }
    }

    public class DeleteCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;
        private List<Notification> _notifications = new List<Notification>();

        public DeleteCategoryCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _productDbContext.Category
                .Include(i => i.Products)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (categoryFromDb == null)
            {
                _notifications.Add(new Notification() { Message = "Category not found", Type = ENotificationType.Error });

                return new DeleteCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = _notifications
                };
            }

            using var transaction = _productDbContext.Database.BeginTransaction();

            if (categoryFromDb.Products.Any() && request.DeleteProducts)
                _productDbContext.Product.RemoveRange(categoryFromDb.Products);

            // In CategoryMapping, the FK config will set Product.CategoryId to null if there are any children to the removed category
            _productDbContext.Remove(categoryFromDb);
            await _productDbContext.SaveChangesAsync();

            _notifications.Add(new Notification() { Message = $"Category {categoryFromDb.Name} deleted.", Type = ENotificationType.Informative });

            transaction.Commit();

            return new DeleteCategoryCommandResponse()
            {
                Success = true,
                Notifications = _notifications
            };
        }
    }
}
