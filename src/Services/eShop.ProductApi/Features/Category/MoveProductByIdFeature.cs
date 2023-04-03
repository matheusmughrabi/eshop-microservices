using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Domain;
using eShop.ProductApi.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpPut("MoveProductById")]
        public async Task<IActionResult> MoveProductById([FromBody] MoveProductByCommand request) => Ok(await _mediator.Send(request));
    }

    public class MoveProductByCommand : IRequest<MoveProductByIdCommandResponse>
    {
        public Guid ProductId { get; set; }
        public Guid DestineCategoryId { get; set; }
    }

    public class MoveProductByIdValidator : AbstractValidator<MoveProductByCommand>
    {
        public MoveProductByIdValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .WithMessage("'ProductId' cannot be null or empty")
                .WithSeverity(Severity.Warning);

            RuleFor(c => c.DestineCategoryId)
                .NotEmpty()
                .WithMessage("'DestineCategoryId' cannot be null or empty")
                .WithSeverity(Severity.Warning);
        }
    }

    public class MoveProductByIdCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class MoveProductByIdCommandHandler : IRequestHandler<MoveProductByCommand, MoveProductByIdCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;
        private List<Notification> _notifications = new List<Notification>();

        public MoveProductByIdCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<MoveProductByIdCommandResponse> Handle(MoveProductByCommand request, CancellationToken cancellationToken)
        {
            await ValidateDestineCategory(request.DestineCategoryId);

            var product = await GetProductById(request.ProductId);

            if(_notifications.Any(c => c.Type == ENotificationType.Warning || c.Type == ENotificationType.Error))
                return new MoveProductByIdCommandResponse(){ Success = false, Notifications = _notifications };

            product?.ChangeCategory(request.DestineCategoryId);

            await _productDbContext.SaveChangesAsync();

            return new MoveProductByIdCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                {
                    new Notification(){ Message = "Product was moved", Type = ENotificationType.Informative }
                }
            };
        }

        private async Task ValidateDestineCategory(Guid destineCategoryId)
        {
            var destineCategoryExists = await _productDbContext.Category.AnyAsync(c => c.Id == destineCategoryId);

            if (!destineCategoryExists)
                _notifications.Add(new Notification() { Message = "Destine category not found", Type = ENotificationType.Warning });
        }

        private async Task<ProductEntity?> GetProductById(Guid productId)
        {
            var product = await _productDbContext.Product
                .FirstOrDefaultAsync(c => c.Id == productId);

            if (product == null)
                _notifications.Add(new Notification() { Message = "Product not found", Type = ENotificationType.Warning });

            return product;
        }
    }
}
