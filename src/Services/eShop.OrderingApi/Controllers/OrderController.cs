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

    [HttpPost("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        command.UserId = User.Identity.Name;
        var response = await _mediator.Send(command);

        if (response.ValidationResult is not null && response.ValidationResult.IsInvalid)
            return BadRequest(response.ValidationResult.Validations);

        return Ok();
    }
}
