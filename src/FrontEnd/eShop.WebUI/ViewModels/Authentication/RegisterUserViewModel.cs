using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShop.WebUI.ViewModels.Authentication;

public class RegisterUserViewModel
{
    [Required]
    public string Username { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [DisplayName("Confirm password")]
    public string ConfirmPassword { get; set; }
}
