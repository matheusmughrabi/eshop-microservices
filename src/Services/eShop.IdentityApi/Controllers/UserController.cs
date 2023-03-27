using eShop.IdentityApi.Models;
using eShop.IdentityApi.Models.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.IdentityApi.Controllers;

[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var identityUser = new IdentityUser()
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var createUserResult = await _userManager.CreateAsync(identityUser, request.Password);

        

        if (createUserResult.Succeeded)
            return Ok(new RegisterUserResponse()
            {
                Notifications = new List<Notification>()
                {
                    new Notification()
                    {
                        Message = "User created successfuly",
                        Type = ENotificationType.Informative
                    }
                }
            });

        return BadRequest(new RegisterUserResponse()
        {
            Notifications = createUserResult.Errors.Select(error => new Notification()
            {
                Message = error.Description,
                Type = ENotificationType.Error
            }).ToList()
        });
    }

    [HttpGet("GetClaims")]
    [Authorize]
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

        return Ok(claims.Select(p => new {Type = p.Type, Value = p.Value}));
    }

    [HttpPost("AddClaims")]
    public async Task<IActionResult> AddClaims([FromBody] AddClaimsRequest request)
    {
        if (request.Claims is null || request.Claims.Count() == 0)
            return BadRequest("At least 1 claim must be provided");

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
            return NotFound();

        var claims = request.Claims.Select(p => new Claim(p.Type, p.Value));
        var result = await _userManager.AddClaimsAsync(user, claims);
        if (result.Succeeded)
            return NoContent();

        return Problem(result.Errors.ToString());
    }
}
