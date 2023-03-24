using eShop.WebUI.Services.ProductApi.Requests;

namespace eShop.WebUI.Services.ProductApi;

public interface IProductApiClient
{
    Task<GetCategoriesResponse> GetCategoriesPaginated(int? page = null, int? itemsPerPage = null, bool paginate = false);
    Task<GetProductsPaginatedResponse> GetProductsPaginated(int page, int itemsPerPage, Guid? categoryId = null);
    Task<GetProductByIdResponse> GetById(Guid id);
    Task<GetProductsResponse> GetProducts(Guid? categoryId = null);
    Task<GetAllCategoryGroupsResponse> GetAllCategoryGroups();
}
