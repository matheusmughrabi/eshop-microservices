namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoryByIdResponse> GetCategoryById(Guid id, int page, int itemsPerPage) => await _genericApiClient.GetAsync<GetCategoryByIdResponse>(ApiKey, $"/api/Category/GetById/{id}?page={page}&itemsPerPage={itemsPerPage}");

        public class GetCategoryByIdResponse
        {
            public int TotalItems { get; set; }
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
            public List<Product> Products { get; set; }

            public class Product
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public decimal Price { get; set; }
            }
        }
    }
}
