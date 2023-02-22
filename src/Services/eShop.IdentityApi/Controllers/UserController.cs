using eShop.IdentityApi.Models.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.IdentityApi.Controllers;

public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("User/Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var identityUser = new IdentityUser()
        {
            UserName = request.Username,
            Email = request.Email
        };

        var createUserResult = await _userManager.CreateAsync(identityUser, request.Password);

        if (createUserResult.Succeeded)
            return Ok();

        return BadRequest(createUserResult.Errors);
    }

    [Authorize]
    [HttpGet("User/GetClaims")]
    public async Task<IActionResult> GetAllClaims()
    {
        var user = await _userManager.FindByIdAsync(User.Identity.Name);
        if (user is null)
            return NotFound();

        var claims = await _userManager.GetClaimsAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return Ok(claims);
    }
}
