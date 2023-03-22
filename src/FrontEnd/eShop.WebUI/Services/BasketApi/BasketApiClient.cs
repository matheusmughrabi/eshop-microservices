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
}
