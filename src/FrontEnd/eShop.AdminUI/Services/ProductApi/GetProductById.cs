using System.Text.Json;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetProductByIdResponse> GetProductById(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var httpResponseMessage = await httpClient.GetAsync($"api/Product/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<GetProductByIdResponse>(response, options);
        }

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
