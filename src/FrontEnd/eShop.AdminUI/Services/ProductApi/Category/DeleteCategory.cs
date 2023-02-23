namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<DeleteCategoryResponse> DeleteCategory(DeleteCategoryRequest request) => await _genericApiClient.DeleteAsync<DeleteCategoryRequest, DeleteCategoryResponse>(ApiKey, "/api/Category/Delete", request);

        public class DeleteCategoryRequest
        {
            public Guid Id { get; set; }
            public bool DeleteProducts { get; set; }
        }

        public class DeleteCategoryResponse
        {
            public bool Success { get; set; }
            public Guid? CategoryId { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
