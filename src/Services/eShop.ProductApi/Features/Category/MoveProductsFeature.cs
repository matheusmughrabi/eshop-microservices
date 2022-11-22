using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpPut("MoveProducts")]
        public async Task<IActionResult> MoveProducts([FromBody] MoveProductsCommand request) => Ok(await _mediator.Send(request));
    }

    public class MoveProductsCommand : IRequest<MoveProductsCommandResponse>
    {
        public Guid OriginalCategoryId { get; set; }
        public Guid DestineCategoryId { get; set; }
    }

    public class MoveProductsValidator : AbstractValidator<MoveProductsCommand>
    {
        public MoveProductsValidator()
        {
            RuleFor(c => c.OriginalCategoryId)
                .NotEmpty()
                .WithMessage("'Id' cannot be null or empty")
                .NotEqual(p => p.DestineCategoryId)
                .WithMessage("'OriginalCategoryId' cannot be equal 'DestineCategoryId'")
                .WithSeverity(Severity.Warning);
        }
    }

    public class MoveProductsCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class MoveProductsCommanddHandler : IRequestHandler<MoveProductsCommand, MoveProductsCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public MoveProductsCommanddHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<MoveProductsCommandResponse> Handle(MoveProductsCommand request, CancellationToken cancellationToken)
        {
            var requestValidator = new MoveProductsValidator();
            var validationResults = requestValidator.Validate(request);

            if (!validationResults.IsValid)
                return new MoveProductsCommandResponse()
                {
                    Success = false,
                    Notifications = validationResults.ToNotifications()
                };

            var category = await _productDbContext.Category
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == request.OriginalCategoryId);

            if (category is null)
            {
                return new MoveProductsCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification(){ Message = "Category not found", Type = ENotificationType.Warning }
                    }
                };
            }

            if (!category.Products.Any())
            {
                return new MoveProductsCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification(){ Message = "No products to be moved in this category", Type = ENotificationType.Warning }
                    }
                };
            }

            var totalProducts = category.Products.Count;

            foreach (var product in category.Products)
            {
                product.ChangeCategory(request.DestineCategoryId);
            }

            await _productDbContext.SaveChangesAsync();

            return new MoveProductsCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                {
                    new Notification(){ Message = totalProducts == 1 ? $"{totalProducts} product moved" : $"{totalProducts} products moved", Type = ENotificationType.Informative }
                }
            };
        }
    }
}
