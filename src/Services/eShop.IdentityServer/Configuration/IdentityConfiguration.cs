using Duende.IdentityServer.Models;

namespace eShop.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("geek_shopping", "GeekShopping Server"),
                new ApiScope("read", "Read data"),
                new ApiScope("write", "write data"),
                new ApiScope("delete", "delete data")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret_key".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile" }
                },
                new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = { new Secret("secret_key".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:7259/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:7259/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        Duende.IdentityServer.IdentityServerConstants.StandardScopes.OpenId,Duende.IdentityServer.IdentityServerConstants.StandardScopes.OpenId,
                        Duende.IdentityServer.IdentityServerConstants.StandardScopes.OpenId,Duende.IdentityServer.IdentityServerConstants.StandardScopes.Profile,
                        Duende.IdentityServer.IdentityServerConstants.StandardScopes.OpenId,Duende.IdentityServer.IdentityServerConstants.StandardScopes.Email,
                        "geek_shopping"
                    },
                }
            };
    }
}
