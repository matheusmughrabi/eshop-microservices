using eShop.WebUI.Services.BasketApi;
using eShop.WebUI.Services.BasketApi.Requests;
using eShop.WebUI.Services.ProductApi;
using eShop.WebUI.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

public class ProductsController : Controller
{
    private readonly IProductApiClient _productApiClient;
    private readonly IBasketApiClient _basketApiClient;

    public ProductsController(
        IProductApiClient productApiClient, 
        IBasketApiClient basketApiClient)
    {
        _productApiClient = productApiClient;
        _basketApiClient = basketApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid? categoryId)
    {
        var productsResponse = await _productApiClient.GetProducts(categoryId);

        var productsViewModel = new IndexViewModel.ProductsViewModel()
        {
            Products = productsResponse.Products.Select(c => new IndexViewModel.ProductsViewModel.Product()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                ImagePath = c.ImagePath
            }).ToList()
        };

        var indexViewModel = new IndexViewModel()
        {
            ProductsVM = productsViewModel
        };

        return View(indexViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(AddToBasketViewModel addToCartViewModel)
    {
        if (User.Identity.IsAuthenticated == false)
        {
            return Json(new
            {
                Status = "401"
            });
        }

        var product = await _productApiClient.GetById(addToCartViewModel.Id);

        var request = new AddToBasketRequest()
        {
            Id = addToCartViewModel.Id,
            Name = product.Name,
            Price = product.Price,
            ImagePath = product.ImagePath,
            Quantity = 1
        };

        var success = await _basketApiClient.AddToBasket(request);

        return Json(new { success = success });
    }
}
