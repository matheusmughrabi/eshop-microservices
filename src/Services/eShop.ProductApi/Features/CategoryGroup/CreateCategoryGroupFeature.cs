using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.ProductApi.Features.CategoryGroup;

public partial class CategoryGroupController
{
    [HttpPost("Create")]
    [Authorize(Policy = "Catalog_Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryGroupCommand request)
    {
        if (string.IsNullOrEmpty(request.Name))
            return BadRequest("Name cannot be null or empty");

        var response = await _mediator.Send(request);

        return Ok(response);
    }
}

public class CreateCategoryGroupCommand : IRequest<bool>
{
    public string Name { get; set; }
}


public class CreateCategoryGroupCommandHandler : IRequestHandler<CreateCategoryGroupCommand, bool>
{
    private readonly ProductDbContext _productDbContext;

    public CreateCategoryGroupCommandHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<bool> Handle(CreateCategoryGroupCommand request, CancellationToken cancellationToken)
    {
        await _productDbContext.CategoryGroup.AddAsync(new Domain.CategoryGroupEntity()
        {
            Name = request.Name
        });

        await _productDbContext.SaveChangesAsync();

        return true;
    }
}
