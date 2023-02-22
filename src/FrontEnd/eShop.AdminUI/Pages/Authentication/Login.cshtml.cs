using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Authentication;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public async Task OnGetAsync()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //var response = await _productApiClient.CreateCategory(CreateCategoryViewModel.MapToCreateRequest());

        //if (!response.Success)
        //    return new JsonResult(response);

        return RedirectToPage("./Index");
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
