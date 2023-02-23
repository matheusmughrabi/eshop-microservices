using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Catalog_Admin")]
    public partial class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
