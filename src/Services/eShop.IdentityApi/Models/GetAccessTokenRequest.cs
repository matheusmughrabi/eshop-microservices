namespace eShop.IdentityApi.Models;

public class GetAccessTokenRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class GetAccessTokenResponse
{
    public string Token { get; set; }
}
