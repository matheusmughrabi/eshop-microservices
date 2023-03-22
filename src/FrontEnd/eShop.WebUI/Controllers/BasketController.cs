using eShop.WebUI.Services.BasketApi;
using eShop.WebUI.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

[Authorize]
public class BasketController : Controller
{
    private readonly IBasketApiClient _basketApiClient;

    public BasketController(IBasketApiClient basketApiClient)
    {
        _basketApiClient = basketApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var basketResponse = await _basketApiClient.GetBasket();
        var basketViewModel = new BasketViewModel()
        {
            Items = basketResponse.Items.Select(c => new BasketViewModel.Item()
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Quantity = c.Quantity,
                ImagePath = c.ImagePath
            }).ToList()
        };

        return View(basketViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(RemoveItemFromBasketViewModel request)
    {
        var success = await _basketApiClient.RemoveItem(new Services.BasketApi.Requests.RemoveItemFromBasketRequest()
        {
            ItemId = request.ItemId
        });

        return Json(new { success = success });
    }
}
