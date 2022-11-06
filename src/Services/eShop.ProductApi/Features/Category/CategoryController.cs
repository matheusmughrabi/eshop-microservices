using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
