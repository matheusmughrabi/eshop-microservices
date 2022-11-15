namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetProductByIdResponse> GetProductById(Guid id) => await _genericApiClient.GetAsync<GetProductByIdResponse>(ApiKey, $"api/Product/{id}");

        public class GetProductByIdResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public Guid CategoryId { get; set; }
        }
    }
}
