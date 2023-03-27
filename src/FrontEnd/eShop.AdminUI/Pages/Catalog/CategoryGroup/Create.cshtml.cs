using eShop.AdminUI.Pages.Catalog.Category;
using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.CategoryGroup;

public class CreateModel : PageModel
{
    private readonly IProductApiClient _productApiClient;

    public CreateModel(IProductApiClient productApiClient)
    {
        _productApiClient = productApiClient;
    }

    [BindProperty]
    public CreateCategoryGroupViewModel CreateCategoryGroupViewModel { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var success = await _productApiClient.CreateCategoryGroup(new ProductApiClient.CreateCategoryGroupRequest()
        {
            Name = CreateCategoryGroupViewModel.Name
        });

        if (!success)
            return new JsonResult(success);

        return RedirectToPage("/catalog/category/Index");
    }
}

public class CreateCategoryGroupViewModel
{
    public string Name { get; set; }
}
