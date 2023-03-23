using eShop.WebUI.Services.Extensions;
using eShop.WebUI.Services.OrderApi.Requests;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.WebUI.Services.OrderApi;

public class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetOrdersResponse> GetOrders()
    {
        var route = "/Order/GetOrders";

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

        return JsonSerializer.Deserialize<GetOrdersResponse>(response, options);
    }

    public async Task<bool> PlaceOrder(PlaceOrderRequest request)
    {
        var content = new StringContent(
           JsonSerializer.Serialize(request),
           Encoding.UTF8,
           Application.Json);

        var token = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token"];

        if (!string.IsNullOrEmpty(token))
            _httpClient.AddAccessToken(token);

        var httpResponseMessage = await _httpClient.PostAsync("/Order/PlaceOrder", content);

        return httpResponseMessage.IsSuccessStatusCode;
    }
}
