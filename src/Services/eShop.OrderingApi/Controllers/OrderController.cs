using eShop.OrderingApi.Application.GetOrders;
using eShop.OrderingApi.Application.PlaceOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.OrderingApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var command = new GetOrdersCommand()
        {
            UserId = User.Identity.Name
        };

        var response = await _mediator.Send(command);

        return Ok(response);
    }
}
