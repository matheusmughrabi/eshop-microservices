using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.WebUI.Services.Product
{
    public partial class ProductApiClient
    {
        public async Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpResponseMessage = await httpClient.PostAsync("api/Category/Create", content);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CreateCategoryResponse>(response);
        }

        public class CreateCategoryRequest
        {
            public string Name { get; set; }
            public string? Description { get; set; }
        }

        public class CreateCategoryResponse
        {
            public bool Success { get; set; }
            public Guid? CategoryId { get; set; }
            public List<Notification> Notifications { get; set; }
        }

        public class Notification
        {
            public string Message { get; set; }
            public ENotificationType Type { get; set; }
        }

        public enum ENotificationType
        {
            Informative = 1,
            Warning = 2,
            Error = 3,
        }
    }
}
