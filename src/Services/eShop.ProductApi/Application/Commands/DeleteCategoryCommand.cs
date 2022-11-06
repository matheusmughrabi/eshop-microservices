using eShop.ProductApi.DataAccess.Repositories;
using eShop.ProductApi.Notifications;
using MediatR;

namespace eShop.ProductApi.Application.Commands
{
    public class DeleteCategoryCommand : IRequest<DeleteCategoryCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCategoryCommandResponse
    {
        public bool Success { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _categoryRepository.GetCategoryById(request.Id);
            if (categoryFromDb == null)
                return new DeleteCategoryCommandResponse()
                {
                    Success = false,
                    Notifications = new List<Notification>()
                    {
                        new Notification() { Message = "Category not found", Type = ENotificationType.Error }
                    }
                };

            await _categoryRepository.DeleteCategory(categoryFromDb);
            await _categoryRepository.SaveChanges();

            return new DeleteCategoryCommandResponse()
            {
                Success = true
            };
        }
    }
}
