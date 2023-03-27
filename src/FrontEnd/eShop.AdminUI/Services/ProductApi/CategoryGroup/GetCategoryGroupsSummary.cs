namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<GetCategoryGroupsSummaryResponse> GetCategoryGroupsSummary() 
            => await _genericApiClient.GetAsync<GetCategoryGroupsSummaryResponse>(ApiKey, $"/api/CategoryGroup/GetSummary");

        public class GetCategoryGroupsSummaryResponse
        {
            public List<CategoryGroup> CategoryGroups { get; set; }

            public class CategoryGroup
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
