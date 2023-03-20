namespace eShop.ProductApi.Services.Blob
{
    public interface IBlobService
    {
        Task CreateContainer(string containerName);
        Task<string> UploadFile(string base64Image, string container);
    }
}
