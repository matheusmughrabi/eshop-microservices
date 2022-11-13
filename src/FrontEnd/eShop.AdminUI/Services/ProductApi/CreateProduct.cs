using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpResponseMessage = await httpClient.PostAsync("api/Product/Create", content);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<CreateProductResponse>(response, options);
        }

        public class CreateProductRequest
        {
            public Guid CategoryId { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
        }

        public class CreateProductResponse
        {
            public bool Success { get; set; }
            public Guid? ProductId { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
