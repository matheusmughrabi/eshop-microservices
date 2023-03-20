namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest request) => await _genericApiClient.PostAsync<CreateProductRequest, CreateProductResponse>(ApiKey, "/api/Product/Create", request);

        public class CreateProductRequest
        {
            public Guid CategoryId { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public string? Base64Image { get; set; }
        }

        public class CreateProductResponse
        {
            public bool Success { get; set; }
            public Guid? ProductId { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
