﻿using eShop.WebUI.Services.BasketApi;
using eShop.WebUI.Services.OrderApi;
using eShop.WebUI.Services.OrderApi.Requests;
using eShop.WebUI.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

[Authorize]
public class BasketController : Controller
{
    private readonly IBasketApiClient _basketApiClient;
    private readonly IOrderApiClient _orderApiClient;

    public BasketController(IBasketApiClient basketApiClient, IOrderApiClient orderApiClient)
    {
        _basketApiClient = basketApiClient;
        _orderApiClient = orderApiClient;
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

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var success = await _basketApiClient.Checkout();

        return Json(new { success = success });
    }
}
