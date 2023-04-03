using eShop.ProductApi.Services.Blob;

namespace eShop.ProductApi.Configuration
{
    public static class BlobSeeder
    {
        public static void CreateBlobContainers(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var blobSeeder = scope.ServiceProvider.GetRequiredService<IBlobService>();

                blobSeeder.CreateContainer("products");
            }
        }
    }
}
