using eShop.WebUI.Services.OrderApi;
using eShop.WebUI.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.WebUI.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderApiClient _orderApiClient;

    public OrderController(IOrderApiClient orderApiClient)
    {
        _orderApiClient = orderApiClient;
    }

    public async Task<IActionResult> OrderPlaced()
    {
        return View();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _orderApiClient.GetOrders();

        var ordersViewModel = new OrderViewModel()
        {
            Orders = response.Orders.Select(c => new OrderViewModel.Order()
            {
                Id = c.Id,
                DateOfPurchase = c.DateOfPurchase,
                Status = c.Status,
                StatusDescription = c.StatusDescription,
                Products = c.Products.Select(p => new OrderViewModel.Product()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    PriceAtPurchase = p.PriceAtPurchase,
                    ImagePath = p.ImagePath
                }).ToList(),
                Notifications = c.Notifications?.Select(p => new OrderViewModel.Notification()
                {
                    Description = p.Description
                }).ToList()
            }).ToList()
        };

        return View(ordersViewModel);
    }
}
