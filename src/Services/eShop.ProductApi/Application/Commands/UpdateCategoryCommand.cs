using eShop.ProductApi.DataAccess.Repositories;
using eShop.ProductApi.Notifications;
using MediatR;

namespace eShop.ProductApi.Application.Commands
{
    public class UpdateCategoryCommand : IRequest<UpdateCategoryCommandResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _categoryRepository.GetCategoryById(request.Id);
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
            await _categoryRepository.SaveChanges();

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
