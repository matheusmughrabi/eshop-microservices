using eShop.ProductApi.DataAccess.Repositories;
using eShop.ProductApi.Entity;
using eShop.ProductApi.Notifications;
using MediatR;

namespace eShop.ProductApi.Application.Commands
{
    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
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
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _categoryRepository.CategoryExists(request.Name);
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

            var createdCategory = await _categoryRepository.CreateCategory(new CategoryEntity(request.Name, request.Description));
            await _categoryRepository.SaveChanges();

            return new CreateCategoryCommandResponse()
            {
                Success = true,
                Notifications = new List<Notification>()
                    {
                        new Notification()
                        {
                            Message = $"Category {createdCategory.Name} created successfuly",
                            Type = ENotificationType.Informative
                        }
                    }
            };
        }
    }
}
