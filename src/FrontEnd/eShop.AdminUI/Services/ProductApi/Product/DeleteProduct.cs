namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request) => await _genericApiClient.DeleteAsync<DeleteProductRequest, DeleteProductResponse>(ApiKey, "api/Product/Delete", request);

        public class DeleteProductRequest
        {
            public Guid Id { get; set; }
        }

        public class DeleteProductResponse
        {
            public bool Success { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
