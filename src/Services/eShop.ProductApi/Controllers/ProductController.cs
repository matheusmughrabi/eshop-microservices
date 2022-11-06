using eShop.ProductApi.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand request) => Ok(await _mediator.Send(request));
    }
}
