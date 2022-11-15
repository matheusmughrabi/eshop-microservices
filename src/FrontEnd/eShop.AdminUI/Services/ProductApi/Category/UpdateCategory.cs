namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request) =>  await _genericApiClient.PutAsync<UpdateCategoryRequest, UpdateCategoryResponse>(ApiKey, "api/Category/Update", request);       

        public class UpdateCategoryRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
        }

        public class UpdateCategoryResponse
        {
            public bool Success { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
