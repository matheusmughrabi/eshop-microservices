namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<MoveProductByIdResponse> MoveProductById(MoveProductByIdRequest request) => await _genericApiClient.PutAsync<MoveProductByIdRequest, MoveProductByIdResponse>(ApiKey, "api/Category/MoveProductById", request);

        public class MoveProductByIdRequest
        {
            public Guid ProductId { get; set; }
            public Guid DestineCategoryId { get; set; }
        }
        public class MoveProductByIdResponse
        {
            public bool Success { get; set; }
            public List<Notification> Notifications { get; set; }
        }
    }
}
