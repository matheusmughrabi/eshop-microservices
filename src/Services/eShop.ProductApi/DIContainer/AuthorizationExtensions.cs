namespace eShop.ProductApi.DIContainer;

public static class AuthorizationExtensions
{
    public static void RegisterAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(config =>
        {
            config.AddPolicy("Catalog_Admin", policyBuilder =>
            {
                policyBuilder.RequireClaim("catalog", "admin");
            });

            config.AddPolicy("Stock_Admin", policyBuilder =>
            {
                policyBuilder.RequireClaim("stock", "admin");
            });
        });
    }
}
