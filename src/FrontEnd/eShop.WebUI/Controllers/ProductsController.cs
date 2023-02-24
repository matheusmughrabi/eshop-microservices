using eShop.WebUI.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

public class ProductsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var productsVM = new ProductsViewModel()
        {
            Products = new List<Product>() 
            { 
                new Product { Id = Guid.NewGuid(), Name = "Teste 1", CategoryId = Guid.NewGuid(), Description = "Desc", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Teste 2", CategoryId = Guid.NewGuid(), Description = "Desc", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Teste 3", CategoryId = Guid.NewGuid(), Description = "Desc", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Teste 4", CategoryId = Guid.NewGuid(), Description = "Desc", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Teste 5", CategoryId = Guid.NewGuid(), Description = "Desc", Price = 100 },
            }
        };

        return View(productsVM);
    }
}
