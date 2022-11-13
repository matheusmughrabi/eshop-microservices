using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Catalog.Category
{
    public class DetailsModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public DetailsModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public CategoryDetailsViewModel CategoryDetailsViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid categoryId)
        {
            var response = await _productApiClient.GetCategoryById(categoryId);

            if(response == null)
                return NotFound();

            CategoryDetailsViewModel = response.MapToCategoryDetailsViewModel();
            return Page();
        }
    }

    public class CategoryDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
        public List<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }
    }

    public static class CategoryDetailsViewModelMaper
    {
        public static CategoryDetailsViewModel MapToCategoryDetailsViewModel(this ProductApiClient.GetCategoryByIdResponse source)
        {
            return new CategoryDetailsViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                TotalProducts = source.TotalProducts,
                Products = source.Products
                    .Select(p => new CategoryDetailsViewModel.Product()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price
                    }).ToList()
            };
        }
    }
}
