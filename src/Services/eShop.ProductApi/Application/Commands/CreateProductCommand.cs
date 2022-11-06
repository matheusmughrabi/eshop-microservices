using eShop.ProductApi.DataAccess.Repositories;
using eShop.ProductApi.Entity;
using eShop.ProductApi.Notifications;
using MediatR;

namespace eShop.ProductApi.Application.Commands
{
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductCommandResponse
    {
        public bool Success { get; set; }
        public Guid? ProductId { get; set; }
        public List<Notification> Notifications { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _categoryRepository.GetCategoryById(id:request.CategoryId, includeProcuts:true);
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

            categoryFromDb.AddProduct(new ProductEntity(request.Name, request.Price, request.Description));
            await _categoryRepository.SaveChanges();

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
