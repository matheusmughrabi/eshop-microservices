namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient
    {
        public async Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request) => await _genericApiClient.PostAsync<CreateCategoryRequest, CreateCategoryResponse>(ApiKey, "api/Category/Create", request);

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
