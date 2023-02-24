using eShop.WebUI.Services.Identity.Requests;

namespace eShop.WebUI.Services.Identity;

public interface IIdentityApiClient
{
    Task<GetAccessTokenResponse> GetAccessToken(GetAccessTokenRequest request);
}
