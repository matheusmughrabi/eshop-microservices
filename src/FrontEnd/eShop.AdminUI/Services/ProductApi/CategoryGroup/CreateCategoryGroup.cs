namespace eShop.AdminUI.Services.ProductApi;

public partial class ProductApiClient
{
    public async Task<bool> CreateCategoryGroup(ProductApiClient.CreateCategoryGroupRequest request) 
        => await _genericApiClient.PostAsync<CreateCategoryGroupRequest, bool>(ApiKey, "/api/CategoryGroup/Create", request);

    public class CreateCategoryGroupRequest
    {
        public string Name { get; set; }
    }
}
