using eShop.AdminUI.Services.Extensions;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.AdminUI.Services.Generic;

public class ApiClient : IApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> GetAsync<TResponse>(string api, string path)
    {
        var httpClient = _httpClientFactory.CreateClient(api);
        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            httpClient.AddAccessToken(token);

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
        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            httpClient.AddAccessToken(token);

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
        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            httpClient.AddAccessToken(token);

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
        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            httpClient.AddAccessToken(token);

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
