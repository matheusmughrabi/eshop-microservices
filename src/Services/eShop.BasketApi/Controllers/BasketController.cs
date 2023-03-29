using eShop.BasketApi.Events;
using eShop.BasketApi.Events.Publishers;
using eShop.BasketApi.Extensions;
using eShop.BasketApi.Models;
using eShop.BasketApi.Requests;
using eShop.EventBus.Events.BasketCheckout;
using eShop.EventBus.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eShop.BasketApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BasketController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private readonly BasketCheckoutEventPublisher _basketCheckoutEventPublisher;

    public BasketController(IDistributedCache cache, BasketCheckoutEventPublisher basketCheckoutEventPublisher)
    {
        _cache = cache;
        _basketCheckoutEventPublisher = basketCheckoutEventPublisher;
    }

    [HttpGet("GetBasket")]
    public async Task<IActionResult> GetBasket()
    {
        var recordId = $"{User.Identity.Name}";
        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket is null)
            return NoContent();

        return Ok(basket);
    }

    [HttpPost("AddItem")]
    public async Task<IActionResult> AddItem([FromBody] BasketModel.Item item)
    {
        var recordId = $"{User.Identity.Name}";

        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket == null)
            basket = new BasketModel();

        basket.AddItem(item);

        // Using a long expiration time here to allow users to have access to their baskets for decent amount of time
        await _cache.SetRecordAsync(recordId, basket, TimeSpan.FromHours(2));

        return Ok();
    }

    [HttpPost("RemoveItem")]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveItemRequest request)
    {
        var recordId = $"{User.Identity.Name}";
        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket == null)
            return NotFound("Basket not found");

        basket.RemoveItem(request.ItemId);

        // I chose to remove the basket altogether from the cache if there are no more items left
        if (basket.Items.Count == 0)
            await _cache.RemoveAsync(recordId);
        else
            await _cache.SetRecordAsync(recordId, basket, TimeSpan.FromHours(2));

        return Ok();
    }

    [HttpPost("SubtractItemQuantity")]
    public async Task<IActionResult> SubtractItemQuantity([FromBody] SubtractItemQuantityRequest request)
    {
        var recordId = $"{User.Identity.Name}";
        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket == null)
            return NotFound("Basket not found");

        basket.SubtractItemQuantity(request.ItemId);

        // I chose to remove the basket altogether from the cache if there are no more items left
        if (basket.Items.Count == 0)
            await _cache.RemoveAsync(recordId);
        else
            await _cache.SetRecordAsync(recordId, basket, TimeSpan.FromHours(2));

        return Ok();
    }

    [HttpPost("RemoveAllItems")]
    public async Task<IActionResult> RemoveAllItems()
    {
        var recordId = $"{User.Identity.Name}";
        await _cache.RemoveAsync(recordId);

        return Ok();
    }

    [HttpPost("Checkout")]
    public async Task<IActionResult> Checkout()
    {
        var recordId = $"{User.Identity.Name}";
        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket is null)
            return NotFound();

        // Clear basket
        await _cache.RemoveAsync(recordId);

        var eventMessage = new BasketCheckoutEventMessage()
        {
            UserId = User.Identity.Name,
            Products = basket.Items.Select(item => new BasketCheckoutEventMessage.Product()
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                ImagePath = item.ImagePath,
                PriceAtPurchase = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        // Publish checkout basket event
        _basketCheckoutEventPublisher.Publish(eventMessage);

        return Ok();
    }
}
