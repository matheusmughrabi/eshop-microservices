
namespace eShop.AdminUI.Services.ProductApi
{
    public interface IProductApiClient
    {
        Task<ProductApiClient.CreateCategoryResponse> CreateCategory(ProductApiClient.CreateCategoryRequest request);
        Task<ProductApiClient.GetCategoriesPaginatedResponse> GetCategoriesPaginated(int page, int itemsPerPage);
        Task<ProductApiClient.GetCategoryByIdResponse> GetCategoryById(Guid id);
        Task<ProductApiClient.UpdateCategoryResponse> UpdateCategory(ProductApiClient.UpdateCategoryRequest request);
    }
}
