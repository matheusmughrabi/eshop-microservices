using eShop.WebUI.Services.ProductApi;
using eShop.WebUI.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductApiClient _productApiClient;

    public ProductsController(IProductApiClient productApiClient)
    {
        _productApiClient = productApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _productApiClient.GetProductsPaginated(1, 50);

        var products = response.Products.Select(c => new ProductsViewModel.Product()
        {
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            ImagePath = c.ImagePath
        }).ToList();

        return View(new ProductsViewModel() { Products = products });
    }
}
