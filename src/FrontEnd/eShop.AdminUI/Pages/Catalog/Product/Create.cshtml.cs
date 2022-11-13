using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Product
{
    public class CreateModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public CreateModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [BindProperty]
        public CreateProductViewModel CreateProductViewModel { get; set; } = new CreateProductViewModel();

        public async Task<IActionResult> OnGetAsync(Guid categoryId)
        {
            CreateProductViewModel.CategoryId = categoryId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _productApiClient.CreateProduct(CreateProductViewModel.MapToCreateProductRequest());
            return RedirectToPage();
        }
    }

    public class CreateProductViewModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public static class CreateProductViewModelMaper
    {
        public static ProductApiClient.CreateProductRequest MapToCreateProductRequest(this CreateProductViewModel source)
        {
            return new ProductApiClient.CreateProductRequest()
            {
                CategoryId = source.CategoryId,
                Name = source.Name,
                Description = source.Description,
                Price = source.Price
            };
        }
    }
}
