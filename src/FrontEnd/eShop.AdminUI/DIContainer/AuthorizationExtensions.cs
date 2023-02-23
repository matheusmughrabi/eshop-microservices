namespace eShop.AdminUI.DIContainer;

public static class AuthorizationExtensions
{
    public static void RegisterAuthorization(this IServiceCollection services) 
    {
        services.AddAuthorization(config =>
        {
            config.AddPolicy("Product_Create", policyBuilder =>
            {
                policyBuilder.RequireClaim("Product", "Create");
            });

            config.AddPolicy("Catalog_Admin", policyBuilder =>
            {
                policyBuilder.RequireClaim("catalog", "admin");
            });
        });
    }
}
