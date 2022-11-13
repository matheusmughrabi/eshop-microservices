namespace eShop.WebUI.Services.Product
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
