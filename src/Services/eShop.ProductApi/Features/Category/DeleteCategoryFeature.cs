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
    }

    public class DeleteCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public DeleteCategoryCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _productDbContext.Category
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (categoryFromDb == null)
                return new DeleteCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification() { Message = "Category not found", Type = ENotificationType.Error }
                    }
                };

            _productDbContext.Remove(categoryFromDb);
            await _productDbContext.SaveChangesAsync();

            return new DeleteCategoryCommandResponse()
            {
                Success = true
            };
        }

    }
}
