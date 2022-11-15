using eShop.AdminUI.Services.Constants;
using eShop.AdminUI.Services.Generic;

namespace eShop.AdminUI.Services.ProductApi
{
    public partial class ProductApiClient : IProductApiClient
    {
        private const string ApiKey = ApiClients.ProductApiKey;
        private readonly IGenericApiClient _genericApiClient;

        public ProductApiClient(IGenericApiClient genericApiClient)
        {
            _genericApiClient = genericApiClient;
        }
    }
}
