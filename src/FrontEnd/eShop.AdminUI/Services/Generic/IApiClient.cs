namespace eShop.AdminUI.Services.Generic
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(string api, string path);
        Task<TResponse> PostAsync<TData, TResponse>(string api, string path, TData request);
        Task<TResponse> PutAsync<TData, TResponse>(string api, string path, TData request);
        Task<TResponse> DeleteAsync<TData, TResponse>(string api, string path, TData request);
    }
}
