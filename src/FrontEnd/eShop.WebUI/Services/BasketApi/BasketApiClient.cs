using eShop.WebUI.Services.BasketApi.Requests;
using eShop.WebUI.Services.Extensions;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.WebUI.Services.BasketApi;

public class BasketApiClient : IBasketApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BasketApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetBasketResponse> GetBasket()
    {
        var route = "/api/Basket/GetBasket";

        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            _httpClient.AddAccessToken(token);

        var httpResponseMessage = await _httpClient.GetAsync(route);
        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            return new GetBasketResponse() { Items = new List<GetBasketResponse.Item>() };

        return JsonSerializer.Deserialize<GetBasketResponse>(response, options);
    }

    public async Task<bool> AddToBasket(AddToBasketRequest request)
    {
        var content = new StringContent(
           JsonSerializer.Serialize(request),
           Encoding.UTF8,
           Application.Json);

        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            _httpClient.AddAccessToken(token);

        var httpResponseMessage = await _httpClient.PostAsync("/api/Basket/AddItem", content);

        return httpResponseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveItem(RemoveItemFromBasketRequest request)
    {
        var content = new StringContent(
          JsonSerializer.Serialize(request),
          Encoding.UTF8,
          Application.Json);

        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            _httpClient.AddAccessToken(token);

        var httpResponseMessage = await _httpClient.PostAsync("/api/Basket/RemoveItem", content);

        return httpResponseMessage.IsSuccessStatusCode;
    }
}
