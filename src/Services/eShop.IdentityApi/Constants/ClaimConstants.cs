namespace eShop.IdentityApi.Constants;

public class ClaimConstants
{
    public static Claim CreateProduct = new Claim() { Type = "Product", Value = "Create" };

    public class Claim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
