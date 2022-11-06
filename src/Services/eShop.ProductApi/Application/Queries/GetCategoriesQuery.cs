using eShop.ProductApi.DataAccess.Repositories;
using eShop.ProductApi.DataAccess.Repositories.QueryModels;
using MediatR;

namespace eShop.ProductApi.Application.Queries
{
    public class GetCategoriesQuery : IRequest<GetCategoriesQueryResponse>
    {
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Page must be an integer equal or greater than 1")]
        public int Page { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "ItemsPerPage must be an integer equal or greater than 1")]
        public int ItemsPerPage { get; set; }
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesQueryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoriesQueryResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetCategories(request.Page, request.ItemsPerPage);
        }
    }
}
