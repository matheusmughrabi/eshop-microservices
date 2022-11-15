using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminUI.Services.Generic
{
    public class GenericApiClient : IGenericApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GenericApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> GetAsync<TResponse>(string api, string path)
        {
            var httpClient = _httpClientFactory.CreateClient(api);

            var httpResponseMessage = await httpClient.GetAsync(path);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(response, options);
        }

        public async Task<TResponse> PostAsync<TData, TResponse>(string api, string path, TData request)
        {
            var httpClient = _httpClientFactory.CreateClient(api);

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpResponseMessage = await httpClient.PostAsync(path, content);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(response, options);
        }

        public async Task<TResponse> PutAsync<TData, TResponse>(string api, string path, TData request)
        {
            var httpClient = _httpClientFactory.CreateClient(api);

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                Application.Json);

            var httpResponseMessage = await httpClient.PutAsync(path, content);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(response, options);
        }

        public async Task<TResponse> DeleteAsync<TData, TResponse>(string api, string path, TData request)
        {
            var httpClient = _httpClientFactory.CreateClient(api);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{httpClient.BaseAddress}{path}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(requestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(response, options);
        }
    }
}
