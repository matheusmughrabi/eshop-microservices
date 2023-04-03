using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Domain;
using eShop.ProductApi.Notifications;
using eShop.ProductApi.Services.Blob;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpPost("Create")]
        //[Authorize(Policy = "Product_Create")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand request) => Ok(await _mediator.Send(request));
    }
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {
        public CreateProductCommand(Guid categoryId, string name, string? description, decimal price)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Price = price;
        }

        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Base64Image { get; set; }
    }

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .WithMessage("'CategoryId' cannot be null or empty")
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

            RuleFor(x => x.Base64Image).Custom((base64Image, context) => {
                Span<byte> buffer = new Span<byte>(new byte[base64Image.Length]);
                var isValidBase64 = Convert.TryFromBase64String(base64Image, buffer, out int bytesParsed);

                if (!isValidBase64)
                    context.AddFailure("Invalid image");
            });
        }
    }

    public class CreateProductCommandResponse
    {
        public bool Success { get; set; }
        public Guid? ProductId { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;
        private readonly IBlobService _blobService;

        public CreateProductCommandHandler(ProductDbContext productDbContext, IBlobService blobService)
        {
            _productDbContext = productDbContext;
            _blobService = blobService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var requestValidator = new CreateProductValidator();
            var validationResults = requestValidator.Validate(request);

            if (!validationResults.IsValid)
                return new CreateProductCommandResponse()
                {
                    Success = false,
                    Notifications = validationResults.ToNotifications()
                };

            var categoryFromDb = await _productDbContext.Category.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == request.CategoryId);
            if (categoryFromDb == null)
                return new CreateProductCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = "Category does not exist",
                            Type = ENotificationType.Error
                        }
                    }
                };

            var product = new ProductEntity(request.Name, request.Price, request.Description);
            if (categoryFromDb.Products.Any(c => c.Equals(product)))
                return new CreateProductCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = $"A product with name {request.Name} already exists in this category",
                            Type = ENotificationType.Error
                        }
                    }
                };

            var imagePath = await _blobService.UploadFile(request.Base64Image, "products");

            categoryFromDb.Products.Add(new ProductEntity(request.Name, request.Price, request.Description, imagePath));
            await _productDbContext.SaveChangesAsync();

            return new CreateProductCommandResponse()
            {
                Success = true,
                ProductId = categoryFromDb.Products.Last().Id,
                Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = "Product created successfuly",
                            Type = ENotificationType.Informative
                        }
                    }
            };
        }
    }
}
