using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpResponseMessage = await httpClient.PutAsync("api/Category/Update", content);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<UpdateCategoryResponse>(response, options);
        }

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
