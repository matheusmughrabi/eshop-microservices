using eShop.WebUI.Services.Identity.Requests;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.WebUI.Services.Identity;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _httpClient;

    public IdentityApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetAccessTokenResponse> GetAccessToken(GetAccessTokenRequest request)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            Application.Json);

        var httpResponseMessage = await _httpClient.PostAsync("/api/Authentication/GetAccessToken", content);
        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<GetAccessTokenResponse>(response, options);
    }
}
