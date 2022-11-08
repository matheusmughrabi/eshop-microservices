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
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCategoryCommand request) => Ok(await _mediator.Send(request));
    }

    public class UpdateCategoryCommand : IRequest<UpdateCategoryCommandResponse>
    {
        public UpdateCategoryCommand(Guid id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("'Id' cannot be null or empty")
                .WithSeverity(Severity.Warning); ;

            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage("'Name' cannot be null or empty.")
                .Length(1, 100)
                .WithMessage("'Name' length is limited to 100 characters.")
                .WithSeverity(Severity.Warning);
        }
    }

    public class UpdateCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public UpdateCategoryCommandHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var requestValidator = new UpdateCategoryValidator();
            var validationResults = requestValidator.Validate(request);

            if (!validationResults.IsValid)
                return new UpdateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = validationResults.ToNotifications()
                };

            var categoryFromDb = await _productDbContext.Category.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (categoryFromDb == null)
                return new UpdateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification() { Message = "Category does not exist", Type = ENotificationType.Error }
                    }
                };

            categoryFromDb.Update(request.Name, request.Description);

            await _productDbContext.SaveChangesAsync();

            return new UpdateCategoryCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                {
                    new Notification() { Message = $"Category updated", Type = ENotificationType.Informative }
                }
            };
        }
    }
}
