using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eShop.AdminUI.Pages.Catalog.Category;

public class CreateModel : PageModel
{
    private readonly IProductApiClient _productApiClient;

    public CreateModel(IProductApiClient productApiClient)
    {
        _productApiClient = productApiClient;
    }

    [BindProperty]
    public CreateCategoryViewModel CreateCategoryViewModel { get; set; }
    public List<SelectListItem> CategoriesDropdown { get; set; }

    public async Task OnGetAsync()
    {
        var response = await _productApiClient.GetCategoryGroupsSummary();

        CategoriesDropdown = new List<SelectListItem>();
        foreach (var categoryGroup in response.CategoryGroups)
        {
            CategoriesDropdown.Add(new SelectListItem() { Value = categoryGroup.Id.ToString(), Text = categoryGroup.Name });
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var response = await _productApiClient.CreateCategory(CreateCategoryViewModel.MapToCreateRequest());

        if(!response.Success)
            return new JsonResult(response);

        return RedirectToPage("./Index");
    }
}

public class CreateCategoryViewModel
{
    public Guid CategoryGroupId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

public static class CreateCategoryViewModelMaper
{
    public static ProductApiClient.CreateCategoryRequest MapToCreateRequest(this CreateCategoryViewModel source)
    {
        return new ProductApiClient.CreateCategoryRequest()
        {
            CategoryGroupId = source.CategoryGroupId,
            Name = source.Name,
            Description = source.Description
        };
    }
}
