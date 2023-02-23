namespace eShop.IdentityApi.Models;

public class AddClaimsRequest
{
    public string Username { get; set; }
    public IEnumerable<Claim> Claims { get; set; }

    public class Claim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
