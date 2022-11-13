using eShop.WebUI.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public ProductController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productApiClient.CreateCategory(new ProductApiClient.CreateCategoryRequest()
            {
                Name = "Shoes 2"
            });

            return View();
        }
    }
}
