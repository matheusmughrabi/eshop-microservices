using System.Text;
using System.Text.Json;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            //var content = new StringContent(
            //    JsonSerializer.Serialize(request),
            //    Encoding.UTF8,
            //    Application.Json);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{httpClient.BaseAddress}api/Product/Delete"),
            };

            var httpResponseMessage = await httpClient.SendAsync(requestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<DeleteProductResponse>(response, options);
        }

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
