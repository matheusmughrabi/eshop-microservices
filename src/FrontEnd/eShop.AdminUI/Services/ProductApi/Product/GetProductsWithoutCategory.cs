namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetProductsWithoutCategoryResponse> GetProductsWithoutCategory(int page, int itemsPerPage) => await _genericApiClient.GetAsync<GetProductsWithoutCategoryResponse>(ApiKey, $"/api/Product/GetProductsWithoutCategory?page={page}&itemsPerPage={itemsPerPage}");

        public class GetProductsWithoutCategoryResponse
        {
            public int TotalItems { get; set; }
            public List<Product> Products { get; set; }

            public class Product
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string? Description { get; set; }
                public decimal Price { get; set; }
            }
        }
    }
}
