namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<MoveProductsResponse> MoveProducts(MoveProductsRequest request) => await _genericApiClient.PutAsync<MoveProductsRequest, MoveProductsResponse>(ApiKey, "api/Category/MoveProducts", request);

        public class MoveProductsRequest
        {
            public Guid OriginalCategoryId { get; set; }
            public Guid DestineCategoryId { get; set; }
        }

        public class MoveProductsResponse
        {
            public bool Success { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
