using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.CategoryGroup;

[Route("api/[controller]")]
[ApiController]
public partial class CategoryGroupController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryGroupController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
