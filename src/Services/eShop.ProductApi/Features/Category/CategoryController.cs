using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.Category
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Catalog_Admin")]
    public partial class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
