using eShop.WebUI.Services.BasketApi;
using eShop.WebUI.Services.Identity;
using eShop.WebUI.Services.ProductApi;

namespace eShop.WebUI.DIContainer;

public static class ServicesExtensions
{
    public static void RegisterApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:IdentityApi"]);
        });

        services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ProductApi"]);
        });

        services.AddHttpClient<IBasketApiClient, BasketApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:BasketApi"]);
        });
    }
}
