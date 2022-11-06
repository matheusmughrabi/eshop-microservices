using eShop.ProductApi.Application.Commands;
using eShop.ProductApi.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery request) => Ok(await _mediator.Send(request));

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand request) => Ok(await _mediator.Send(request));

        [HttpPut("Delete")]
        public async Task<IActionResult> UpdateCategory(DeleteCategoryCommand request) => Ok(await _mediator.Send(request));
    }
}
