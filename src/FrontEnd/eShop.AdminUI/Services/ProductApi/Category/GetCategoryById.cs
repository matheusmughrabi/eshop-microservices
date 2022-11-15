namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoryByIdResponse> GetCategoryById(Guid id) => await _genericApiClient.GetAsync<GetCategoryByIdResponse>(ApiKey, $"api/Category/GetById/{id}");

        public class GetCategoryByIdResponse
        {
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
