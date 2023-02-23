using eShop.IdentityApi.Constants;
using eShop.IdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShop.IdentityApi.Controllers;

[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signManager;

    public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager)
    {
        _userManager = userManager;
        _signManager = signManager;
    }

    [HttpPost("GetAccessToken")]
    public async Task<IActionResult> GetAccessToken([FromBody] GetAccessTokenRequest request)
    {
        var signInResult = await _signManager.PasswordSignInAsync(request.Username, request.Password, false, true);

        if (!signInResult.Succeeded)
            return Unauthorized("Username or password incorrect");

        var user = await _userManager.FindByNameAsync(request.Username);

        var claims = await _userManager.GetClaimsAsync(user);
        claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));

        var secretEncoded = Encoding.UTF8.GetBytes(TokenConstants.Secret);
        var securityKey = new SymmetricSecurityKey(secretEncoded);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            TokenConstants.Issuer,
            TokenConstants.Audience,
            claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials
            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new GetAccessTokenResponse() { Token = tokenString };

        return Ok(response);
    }
}
