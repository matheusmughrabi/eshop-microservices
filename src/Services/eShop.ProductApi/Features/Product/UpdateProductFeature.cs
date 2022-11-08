using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Notifications;
using FluentValidation;
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
        public UpdateProductCommand(Guid productId, string name, string? description, decimal price)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .WithMessage("'ProductId' cannot be null or empty")
                .WithSeverity(Severity.Warning);

            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("'Name' cannot be null or empty.")
                .Length(1, 100)
                .WithMessage("'Name' length is limited to 100 characters.")
                .WithSeverity(Severity.Warning);

            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage("'Price' must be greater than zero.")
                .WithSeverity(Severity.Warning);
        }
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
            var requestValidator = new UpdateProductValidator();
            var validationResults = requestValidator.Validate(request);

            if (!validationResults.IsValid)
                return new UpdateProductCommandResponse()
                {
                    Success = false,
                    Notifications = validationResults.ToNotifications()
                };

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

            productFromDb.Update(request.Name, request.Price, request.Description);

            await _productDbContext.SaveChangesAsync();

            return new UpdateProductCommandResponse()
            {
                Success = true
            };
        }
    }
}
