using eShop.AdminUI.Services.ProductApi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace eShop.AdminUI.Pages.Catalog.Pendencies
{
    public class IndexModel : PageModel
    {
        private readonly IProductApiClient _productApiClient;

        public IndexModel(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public ProductsWithoutCategoryViewModel ProductsWithoutCategoryViewModel { get; set; } = new ProductsWithoutCategoryViewModel();
        public List<SelectListItem> CategoriesDropDown { get; set; }

        public async Task OnGet(int selectedpage = 1)
        {
            var itemsPerPage = 3;
            var response = await _productApiClient.GetProductsWithoutCategory(selectedpage, itemsPerPage);
            ProductsWithoutCategoryViewModel.Products = response.MapToProductsWithoutCategoryViewModel(selectedpage, itemsPerPage);

            var categories = await _productApiClient.GetCategoriesPaginated(1, 100);

            CategoriesDropDown = new List<SelectListItem>();
            foreach (var category in categories.Categories)
            {
                CategoriesDropDown.Add(new SelectListItem() { Value = category.Id.ToString(), Text = category.Name });
            }
        }
    }

    public class ProductsWithoutCategoryViewModel
    {
        public IPagedList<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }
    }

    public static class ProductsWithoutCategoryViewModelMaper
    {
        public static StaticPagedList<ProductsWithoutCategoryViewModel.Product> MapToProductsWithoutCategoryViewModel(this ProductApiClient.GetProductsWithoutCategoryResponse source, int selectedPage, int itemsPerPage)
        {
            var products = source.Products
                .Select(p => new ProductsWithoutCategoryViewModel.Product()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                })
                .ToList();

            return new StaticPagedList<ProductsWithoutCategoryViewModel.Product>(products, selectedPage, itemsPerPage, source.TotalItems);
        }
    }
}
