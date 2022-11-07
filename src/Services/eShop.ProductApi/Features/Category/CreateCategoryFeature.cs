using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Domain.Validations;
using eShop.ProductApi.Entity;
using eShop.ProductApi.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateCategoryCommand request) => Ok(await _mediator.Send(request));
    }

    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
        public CreateCategoryCommand(string name, string? description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
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
            var _dataValidations = new List<Notification>();
            _dataValidations.ValidateIfNullOrEmptyName(request.Name);
            _dataValidations.ValidateIfNameIsTooLong(request.Name);

            if (_dataValidations.Count > 0)
                return new CreateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = _dataValidations
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

            var categoryEntity = new CategoryEntity(request.Name, request.Description);
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
