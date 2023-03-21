using eShop.WebUI.Services.ProductApi.Requests;
using System.Text.Json;

namespace eShop.WebUI.Services.ProductApi;

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
