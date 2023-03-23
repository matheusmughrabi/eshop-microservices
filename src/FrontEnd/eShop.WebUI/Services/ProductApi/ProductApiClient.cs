using eShop.WebUI.Services.Extensions;
using eShop.WebUI.Services.ProductApi.Requests;
using System.Text.Json;

namespace eShop.WebUI.Services.ProductApi;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetProductByIdResponse> GetById(Guid id)
    {
        var route = $"/api/Product/{id}";

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

        return JsonSerializer.Deserialize<GetProductByIdResponse>(response, options);
    }

    public async Task<GetProductsResponse> GetProducts(Guid? categoryId = null)
    {
        var categoryIdQueryParam = new QueryParamModel() { Key = "CategoryId", Value = categoryId.ToString() };
        var queryParamsString = GetQueryParams(categoryIdQueryParam);
        var route = $"/api/Product/GetProducts" + queryParamsString;

        var httpResponseMessage = await _httpClient.GetAsync(route);
        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<GetProductsResponse>(response, options);
    }

    public async Task<GetProductsPaginatedResponse> GetProductsPaginated(int page, int itemsPerPage, Guid? categoryId = null)
    {
        var pageQueryParam = new QueryParamModel() { Key = "Page", Value = page.ToString() };
        var itemsPerPageQueryParam = new QueryParamModel() { Key = "ItemsPerPage", Value = itemsPerPage.ToString() };
        var categoryIdQueryParam = new QueryParamModel() { Key = "CategoryId", Value = categoryId?.ToString() };

        var queryParamsString = GetQueryParams(pageQueryParam, itemsPerPageQueryParam, categoryIdQueryParam);

        var route = "/api/Product/GetPaginated" + queryParamsString;

        var httpResponseMessage = await _httpClient.GetAsync(route);
        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<GetProductsPaginatedResponse>(response, options);
    }

    private string GetQueryParams(params QueryParamModel[] queryParams)
    {
        var result = "";
        foreach (var queryParam in queryParams)
        {
            if (queryParam.Key is null || queryParam.Value is null)
                continue;

            if (result == "")
            {
                result = $"?{queryParam.Key}={queryParam.Value}";
            }
            else
            {
                result += $"&{queryParam.Key}={queryParam.Value}";
            }
        }

        return result;
    }
}

public class QueryParamModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
