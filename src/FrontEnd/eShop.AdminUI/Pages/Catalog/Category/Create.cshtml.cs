using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Category
{
    public class CreateModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public CreateModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [BindProperty]
        public CreateCategoryViewModel CreateCategoryViewModel { get; set; }

        public async Task OnGetAsync()
        {
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
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public static class CreateCategoryViewModelMaper
    {
        public static ProductApiClient.CreateCategoryRequest MapToCreateRequest(this CreateCategoryViewModel source)
        {
            return new ProductApiClient.CreateCategoryRequest()
            {
                Name = source.Name,
                Description = source.Description
            };
        }
    }
}
