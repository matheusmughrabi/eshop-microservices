using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.AdminUI.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public IndexModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [BindProperty]
        public CategoriesViewModel CategoriesViewModel { get; set; } = new CategoriesViewModel();

        public async Task OnGet()
        {
            var response = await _productApiClient.GetCategoriesPaginated(1, 10);

            CategoriesViewModel.Categories = response.Categories
                .Select(c => new CategoriesViewModel.Category()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TotalProducts = c.TotalProducts
                }).ToList();
        }
    }

    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public class Category
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
        }
    }
}
