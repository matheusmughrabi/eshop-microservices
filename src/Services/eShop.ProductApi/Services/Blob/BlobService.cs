using Azure.Storage.Blobs;
using System.IO;
using System.Text.RegularExpressions;

namespace eShop.ProductApi.Services.Blob
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task CreateContainer(string containerName)
        {
            try
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.CreateIfNotExistsAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> UploadFile(string base64Image, string container)
        {
            var base64ImageCleaned = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
            byte[] imageBytes = Convert.FromBase64String(base64ImageCleaned);
            var imageName = Guid.NewGuid().ToString() + ".jpg";

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);

            using (var stream = new MemoryStream(imageBytes))
            {
                await blobContainerClient.UploadBlobAsync(imageName, stream);
            }

            return $"{blobContainerClient.Uri.AbsoluteUri}/{imageName}";
        }
    }
}
