using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

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
        public List<SelectListItem> CategoriesDropDown { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid categoryId, int selectedPage = 1)
        {
            var itemsPerPage = 5;
            var response = await _productApiClient.GetCategoryById(categoryId, selectedPage, itemsPerPage);

            if(response == null)
                return NotFound();

            CategoryDetailsViewModel = response.MapToCategoryDetailsViewModel(selectedPage, itemsPerPage);

            var categories = await _productApiClient.GetCategoriesPaginated(1, 100);

            CategoriesDropDown = new List<SelectListItem>();
            foreach (var category in categories.Categories.Where(c => c.Id != categoryId))
            {
                CategoriesDropDown.Add(new SelectListItem() { Value = category.Id.ToString(), Text = category.Name });
            }

            return Page();
        }

        public async Task<JsonResult> OnPostMoveProducts([FromBody] ProductApiClient.MoveProductsRequest request)
        {
            var response = await _productApiClient.MoveProducts(request);
            return new JsonResult(response);
        }

        public async Task<JsonResult> OnPostMoveProductById([FromBody] ProductApiClient.MoveProductByIdRequest request)
        {
            var response = await _productApiClient.MoveProductById(request);
            return new JsonResult(response);
        }
    }

    public class CategoryDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
        public IPagedList<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImagePath { get; set; }
        }
    }

    public static class CategoryDetailsViewModelMaper
    {
        public static CategoryDetailsViewModel MapToCategoryDetailsViewModel(this ProductApiClient.GetCategoryByIdResponse source, int selectedPage, int itemsPerPage)
        {
            var productList = source.Products
                    .Select(p => new CategoryDetailsViewModel.Product()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImagePath = p.ImagePath
                    }).ToList();

            return new CategoryDetailsViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                TotalProducts = source.TotalProducts,
                Products = new StaticPagedList<CategoryDetailsViewModel.Product>(productList, selectedPage, itemsPerPage, source.TotalItems)
            };
        }
    }
}
