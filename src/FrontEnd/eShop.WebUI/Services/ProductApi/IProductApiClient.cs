using eShop.WebUI.Services.ProductApi.Requests;

namespace eShop.WebUI.Services.ProductApi;

public interface IProductApiClient
{
    Task<GetProductsPaginatedResponse> GetProductsPaginated(int page, int itemsPerPage, Guid? categoryId = null);
    Task<GetProductByIdResponse> GetById(Guid id);
}
