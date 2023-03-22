using eShop.WebUI.Services.BasketApi;
using eShop.WebUI.Services.BasketApi.Requests;
using eShop.WebUI.Services.ProductApi;
using eShop.WebUI.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Index()
    {
        var response = await _productApiClient.GetProductsPaginated(1, 50);

        var products = response.Products.Select(c => new ProductsViewModel.Product()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            ImagePath = c.ImagePath
        }).ToList();

        return View(new ProductsViewModel() { Products = products });
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

        var request = new AddToBasketRequest()
        {
            Id = addToCartViewModel.Id,
            Name = addToCartViewModel.Name,
            ImagePath = addToCartViewModel.ImagePath,
            Price = addToCartViewModel.Price,
            Quantity = 1
        };

        var success = await _basketApiClient.AddToBasket(request);

        return Json(new { success = success });
    }
}
