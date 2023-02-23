namespace eShop.AdminUI.Services.IdentityApi;

public interface IIdentityApiClient
{
    Task<IdentityApiClient.GetAccessTokenResponse> GetAccessToken(IdentityApiClient.GetAccessTokenRequest request);
}
