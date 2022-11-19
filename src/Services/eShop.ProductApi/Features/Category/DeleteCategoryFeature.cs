using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Entity;
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
        public Guid? CategoryIdToMoveProducts { get; set; }
    }

    public class DeleteCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;
        private readonly IMediator _mediator;
        private List<Notification> _notifications = new List<Notification>();

        public DeleteCategoryCommandHandler(ProductDbContext productDbContext, IMediator mediator)
        {
            _productDbContext = productDbContext;
            _mediator = mediator;
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

            if (categoryFromDb.Products.Any())
                await HandleProducts(request, categoryFromDb);

            _productDbContext.Remove(categoryFromDb);
            await _productDbContext.SaveChangesAsync();

            transaction.Commit();

            return new DeleteCategoryCommandResponse()
            {
                Success = true,
                Notifications = _notifications
            };
        }

        private async Task HandleProducts(DeleteCategoryCommand request, CategoryEntity categoryFromDb)
        {
            if (request.DeleteProducts)
                _productDbContext.Product.RemoveRange(categoryFromDb.Products);

            else if (request.CategoryIdToMoveProducts is not null)
                await MoveProductsToSpecifiedCategory(categoryFromDb, request.CategoryIdToMoveProducts);

            else
                await MoveProductsToOthersCategory(categoryFromDb);
        }

        private async Task MoveProductsToSpecifiedCategory(CategoryEntity categoryFromDb, Guid? categoryIdToMoveProducts)
        {
            var categoryToMove = await _productDbContext.Category
                .AsNoTracking()
                .Where(c => c.Id == categoryIdToMoveProducts)
                .Select(p => new CategoryModel()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

            if (categoryToMove is not null)
            {
                categoryFromDb.Products.ForEach(p => p.ChangeCategory(categoryToMove.Id));
                await _productDbContext.SaveChangesAsync();
                _notifications.Add(new Notification() { Message = $"Products moved to {categoryToMove.Name} category", Type = ENotificationType.Informative });
            }
            else
            {
                throw new Exception($"Category with id {categoryIdToMoveProducts} not found");
            }
        }

        private async Task MoveProductsToOthersCategory(CategoryEntity categoryFromDb)
        {
            var categoryId = await GetOthersCategoryId();

            categoryFromDb.Products.ForEach(p => p.ChangeCategory(categoryId));
            await _productDbContext.SaveChangesAsync();

            _notifications.Add(new Notification() { Message = $"Products from {categoryFromDb.Name} were moved to 'Others' category", Type = ENotificationType.Informative });
        }

        private async Task<Guid> GetOthersCategoryId()
        {
            var response = await _mediator.Send(new CreateCategoryCommand("Others", "Other products"));

            if (response.CategoryId is null)
            {
                return (await _productDbContext.Category
                            .AsNoTracking()
                            .Where(c => c.Name == "Others")
                            .Select(c => new CategoryModel { Id = c.Id })
                            .SingleAsync())
                            .Id;
                
            }

            _notifications.Add(new Notification() { Message = $"'Others' category was created automatically", Type = ENotificationType.Informative });
            return response.CategoryId.GetValueOrDefault();
        }

        private class CategoryModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

    }
}
