using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Category
{
    public class EditModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public EditModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [BindProperty]
        public UpdateCategoryViewModel UpdateCategoryViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var category = await _productApiClient.GetCategoryById(id, 1, 0);
            if (category == null)
                return NotFound();

            UpdateCategoryViewModel = category.MapToViewModel();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _productApiClient.UpdateCategory(UpdateCategoryViewModel.MapToUpdateRequest());
            if (!response.Success)
                return new JsonResult(response);

            return RedirectToPage("./Index");
        }
    }

    public class UpdateCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public static class UpdateCategoryViewModelMaper
    {
        public static UpdateCategoryViewModel MapToViewModel(this ProductApiClient.GetCategoryByIdResponse source)
        {
            return new UpdateCategoryViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description
            };
        }

        public static ProductApiClient.UpdateCategoryRequest MapToUpdateRequest(this UpdateCategoryViewModel source)
        {
            return new ProductApiClient.UpdateCategoryRequest()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description
            };
        }
    }
}
