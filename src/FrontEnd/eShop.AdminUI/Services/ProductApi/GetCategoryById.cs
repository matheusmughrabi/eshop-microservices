using System.Text.Json;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoryByIdResponse> GetCategoryById(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var httpResponseMessage = await httpClient.GetAsync($"api/Category/GetById/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<GetCategoryByIdResponse>(response, options);
        }

        public class GetCategoryByIdResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
        }
    }
}
