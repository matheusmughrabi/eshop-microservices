
using static eShop.AdminUI.Services.ProductApi.ProductApiClient;

namespace eShop.AdminUI.Services.ProductApi
{
    public interface IProductApiClient
    {
        Task<ProductApiClient.CreateCategoryResponse> CreateCategory(ProductApiClient.CreateCategoryRequest request);
        Task<ProductApiClient.GetCategoriesPaginatedResponse> GetCategoriesPaginated(int page, int itemsPerPage, bool paginate);
        Task<ProductApiClient.GetCategoryByIdResponse> GetCategoryById(Guid id, int page, int itemsPerPage);
        Task<ProductApiClient.UpdateCategoryResponse> UpdateCategory(ProductApiClient.UpdateCategoryRequest request);
        Task<ProductApiClient.CreateProductResponse> CreateProduct(ProductApiClient.CreateProductRequest request);
        Task<ProductApiClient.UpdateProductResponse> UpdateProduct(ProductApiClient.UpdateProductRequest request);
        Task<ProductApiClient.GetProductByIdResponse> GetProductById(Guid id);
        Task<ProductApiClient.DeleteProductResponse> DeleteProduct(ProductApiClient.DeleteProductRequest request);
        Task<ProductApiClient.DeleteCategoryResponse> DeleteCategory(ProductApiClient.DeleteCategoryRequest request);
        Task<ProductApiClient.MoveProductsResponse> MoveProducts(ProductApiClient.MoveProductsRequest request);
        Task<ProductApiClient.MoveProductByIdResponse> MoveProductById(ProductApiClient.MoveProductByIdRequest request);
        Task<ProductApiClient.GetProductsWithoutCategoryResponse> GetProductsWithoutCategory(int page, int itemsPerPage);
        Task<bool> CreateCategoryGroup(ProductApiClient.CreateCategoryGroupRequest request);
        Task<GetCategoryGroupsSummaryResponse> GetCategoryGroupsSummary();
    }
}
