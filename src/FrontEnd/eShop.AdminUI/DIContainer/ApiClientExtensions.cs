using eShop.AdminUI.Services.Constants;
using eShop.AdminUI.Services.Generic;
using eShop.AdminUI.Services.ProductApi;

namespace eShop.AdminUI.Container
{
    public static class ApiClientExtensions
    {
        public static void RegisterHttpClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IGenericApiClient, GenericApiClient>();

            serviceCollection
                .AddScoped<IProductApiClient, ProductApiClient>()
                .AddHttpClient(ApiClients.ProductApiKey, httpClient =>
                    {
                        httpClient.BaseAddress = new Uri(configuration.GetServicesSection(ApiClients.ProductApiKey));
                    });

        }

        private static string GetServicesSection(this IConfiguration configuration, string key)
        {
            return configuration.GetSection("Services")[key] ?? throw new KeyNotFoundException("Key not found");
        }
    }
}
