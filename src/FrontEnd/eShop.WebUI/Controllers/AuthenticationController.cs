using eShop.WebUI.Services.Identity;
using eShop.WebUI.ViewModels.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eShop.WebUI.Controllers;

public class AuthenticationController : Controller
{
    private readonly IIdentityApiClient _identityApiClient;

    public AuthenticationController(IIdentityApiClient identityApiClient)
    {
        _identityApiClient = identityApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginViewModel request)
    {
        var response = await _identityApiClient.GetAccessToken(new Services.Identity.Requests.GetAccessTokenRequest() 
        { 
            Username = request.Username, 
            Password = request.Password 
        });

        Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

        return RedirectToAction("Index", "Products");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Products");
    }
}
