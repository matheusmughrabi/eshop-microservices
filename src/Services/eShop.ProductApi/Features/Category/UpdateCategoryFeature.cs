using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Domain.Validations;
using eShop.ProductApi.Notifications;
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
            var _dataValidations = new List<Notification>();
            _dataValidations.ValidateIfNullOrEmptyName(request.Name);
            _dataValidations.ValidateIfNameIsTooLong(request.Name);

            if (_dataValidations.Count > 0)
                return new UpdateCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = _dataValidations
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
