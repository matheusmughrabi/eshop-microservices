using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Entity;
using eShop.ProductApi.Notifications;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand request) => Ok(await _mediator.Send(request));
    }

    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(Guid categoryGroupId, string name, string? description)
        {
            CategoryGroupId = categoryGroupId;
            Name = name;
            Description = description;
        }

        public Guid CategoryGroupId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("'Name' cannot be null or empty.")
                .Length(1, 100)
                .WithMessage("'Name' length is limited to 100 characters.")
                .WithSeverity(Severity.Warning);
        }
    }

    public class CreateCategoryCommandResponse
    {
        public bool Success { get; set; }
        public Guid? CategoryId { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public CreateCategoryCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var requestValidator = new CreateCategoryValidator();
            var validationResults = requestValidator.Validate(request);

            if (!validationResults.IsValid)
                return new CreateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = validationResults.ToNotifications()
                };

            var categoryExists = await CategoryExists(request.Name);
            if (categoryExists)
                return new CreateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = "A category with this name already exists",
                            Type = ENotificationType.Warning
                        }
                    }
                };

            var categoryEntity = new CategoryEntity(request.Name, request.Description, request.CategoryGroupId);
            await _productDbContext.Category.AddAsync(categoryEntity);
            await _productDbContext.SaveChangesAsync();

            return new CreateCategoryCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = $"Category '{categoryEntity.Name}' created successfuly",
                            Type = ENotificationType.Informative
                        }
                    },
                CategoryId = categoryEntity.Id
            };
        }

        private async Task<bool> CategoryExists(string name)
        {
            return await _productDbContext.Category
                .AsNoTracking()
                .AnyAsync(c => c.Name == name);
        }
    }
}
