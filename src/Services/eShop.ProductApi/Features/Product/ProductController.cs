using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
