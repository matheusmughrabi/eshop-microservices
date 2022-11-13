
namespace eShop.WebUI.Services.Product
{
    public interface IProductApiClient
    {
        Task<ProductApiClient.CreateCategoryResponse> CreateCategory(ProductApiClient.CreateCategoryRequest request);
    }
}
