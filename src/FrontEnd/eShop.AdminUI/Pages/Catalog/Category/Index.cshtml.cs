using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace eShop.AdminUI.Pages.Catalog.Category
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

        public async Task OnGetAsync(int selectedPage = 1)
        {
            var itemsPerPage = 10;
            var response = await _productApiClient.GetCategoriesPaginated(selectedPage, itemsPerPage, paginate: true);

            var categoriesList = response.Categories
                .Select(p => new CategoriesViewModel.Category()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    TotalProducts = p.TotalProducts,
                    CategoryGroupName = p.CategoryGroupName
                })
                .ToList();

            CategoriesViewModel.Categories = new StaticPagedList<CategoriesViewModel.Category>(categoriesList, selectedPage, itemsPerPage, response.TotalItems);
        }

        public async Task<IActionResult> OnPostDeleteCategory([FromBody] ProductApiClient.DeleteCategoryRequest data)
        {
            var response = await _productApiClient.DeleteCategory(data);
            return new JsonResult(response);
        }
    }

    public class CategoriesViewModel
    {
        public IPagedList<Category> Categories { get; set; }

        public class Category
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
            public string? CategoryGroupName { get; set; }
        }
    }
}
