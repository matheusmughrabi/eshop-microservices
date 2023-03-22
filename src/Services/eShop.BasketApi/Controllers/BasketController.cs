using eShop.BasketApi.Extensions;
using eShop.BasketApi.Models;
using eShop.BasketApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace eShop.BasketApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BasketController : ControllerBase
{
    private readonly IDistributedCache _cache;

    public BasketController(IDistributedCache cache)
    {
        _cache = cache;
    }

    [HttpGet("GetBasket")]
    public async Task<IActionResult> GetBasket()
    {
        var recordId = $"Basket_{User.Identity.Name}";
        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        return Ok(basket);
    }

    [HttpPost("AddItem")]
    public async Task<IActionResult> AddItem([FromBody] BasketModel.Item item)
    {
        var recordId = $"Basket_{User.Identity.Name}";

        var basket = await _cache.GetRecordAsync<BasketModel>(recordId);

        if (basket == null)
            basket = new BasketModel();

        basket.AddItem(item);

        // Using a long expiration time here to allow users to have access to their baskets for decent amount of time
        await _cache.SetRecordAsync(recordId, basket, TimeSpan.FromHours(2)); 

        return Ok(new { RecordId = recordId });
    }

    [HttpPost("RemoveItem")]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveItemRequest request)
    {
        var recordId = $"Basket_{User.Identity.Name}";
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
        var recordId = $"Basket_{User.Identity.Name}";
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
        var recordId = $"Basket_{User.Identity.Name}";
        await _cache.RemoveAsync(recordId);

        return Ok();
    }
}
