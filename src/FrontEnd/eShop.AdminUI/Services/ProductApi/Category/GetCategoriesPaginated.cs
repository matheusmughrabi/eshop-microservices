using X.PagedList;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoriesPaginatedResponse> GetCategoriesPaginated(int page, int itemsPerPage) => await _genericApiClient.GetAsync<GetCategoriesPaginatedResponse>(ApiKey, $"api/Category/GetPaginated?page={page}&itemsPerPage={itemsPerPage}");

        public class GetCategoriesPaginatedResponse
        {
            public int TotalItems { get; set; }
            public List<Category> Categories { get; set; }

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
