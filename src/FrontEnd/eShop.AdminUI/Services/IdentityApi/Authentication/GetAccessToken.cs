namespace eShop.AdminUI.Services.IdentityApi
{
    public partial class IdentityApiClient
    {
        public async Task<GetAccessTokenResponse> GetAccessToken(GetAccessTokenRequest request) => await _genericApiClient.PostAsync<GetAccessTokenRequest, GetAccessTokenResponse>(ApiKey, "/api/Authentication/GetAccessToken", request);

        public class GetAccessTokenRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class GetAccessTokenResponse
        {
            public string Token { get; set; }
        }
    }
}
