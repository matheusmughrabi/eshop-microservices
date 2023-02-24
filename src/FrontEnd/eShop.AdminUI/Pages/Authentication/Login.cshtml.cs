using eShop.AdminUI.Services.IdentityApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Authentication;

public class LoginModel : PageModel
{
    private readonly IIdentityApiClient _identityApiClient;

    public LoginModel(IIdentityApiClient identityApiClient)
    {
        _identityApiClient = identityApiClient;
    }

    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public async Task OnGetAsync()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var response = await _identityApiClient.GetAccessToken(new IdentityApiClient.GetAccessTokenRequest()
        {
            Username = LoginRequest.Username,
            Password = LoginRequest.Password
        });

        Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

        return RedirectToPage("/Catalog/Category/Index");
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
