namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request) => await _genericApiClient.PutAsync<UpdateProductRequest, UpdateProductResponse>(ApiKey, "/api/Product/Update", request);

        public class UpdateProductRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class UpdateProductResponse
        {
            public bool Success { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
