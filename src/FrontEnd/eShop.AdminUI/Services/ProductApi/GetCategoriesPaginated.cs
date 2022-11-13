using System.Text.Json;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoriesPaginatedResponse> GetCategoriesPaginated(int page, int itemsPerPage)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            var httpResponseMessage = await httpClient.GetAsync($"api/Category/GetPaginated?page={page}&itemsPerPage={itemsPerPage}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<GetCategoriesPaginatedResponse>(response, options);
        }

        public class GetCategoriesPaginatedResponse
        {
            public List<Category> Categories { get; set; } = new List<Category>();

            public class Category
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string? Description { get; set; }
                public int TotalProducts { get; set; }
            }
        }
    }
}
