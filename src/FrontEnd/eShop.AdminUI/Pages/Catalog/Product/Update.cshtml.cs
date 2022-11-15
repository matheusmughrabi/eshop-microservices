using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Product
{
    public class UpdateModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public UpdateModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [BindProperty]
        public UpdateProductViewModel UpdateProductViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _productApiClient.GetProductById(id);
            if (response == null)
                return NotFound();

            UpdateProductViewModel = response.MapToUpdateProductViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _productApiClient.UpdateProduct(UpdateProductViewModel.MapToUpdateProductRequest());
            if (!response.Success)
                return new JsonResult(response);

            return RedirectToPage("/Catalog/Category/Details", new { categoryId = UpdateProductViewModel.CategoryId });
        }
    }

    public class UpdateProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }

    public static class UpdateProductViewModelMaper
    {
        public static ProductApiClient.UpdateProductRequest MapToUpdateProductRequest(this UpdateProductViewModel source)
        {
            return new ProductApiClient.UpdateProductRequest()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                CategoryId = source.CategoryId
            };
        }

        public static UpdateProductViewModel MapToUpdateProductViewModel(this ProductApiClient.GetProductByIdResponse source)
        {
            return new UpdateProductViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price,
                CategoryId = source.CategoryId
            };
        }
    }
}
