namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient : IProductApiClient
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public ProductApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
