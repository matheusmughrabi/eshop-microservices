using eShop.AdminUI.Services.Constants;
using eShop.AdminUI.Services.Generic;

namespace eShop.AdminUI.Services.IdentityApi;

public partial class IdentityApiClient : IIdentityApiClient
{
    private const string ApiKey = ApiClients.IdentityApiKey;
    private readonly IApiClient _genericApiClient;

    public IdentityApiClient(IApiClient genericApiClient)
    {
        _genericApiClient = genericApiClient;
    }
}
