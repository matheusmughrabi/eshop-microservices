using System.Net.Http.Headers;

namespace eShop.WebUI.Services.Extensions;

public static class HttpClientExtensions
{
    public static void AddAccessToken(this HttpClient client, string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}
