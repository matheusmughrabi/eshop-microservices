using eShop.ProductApi.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.CategoryGroup;

[Route("api/[controller]")]
[ApiController]
public class CategoryGroupController : ControllerBase
{
    private readonly ProductDbContext _productDbContext;

    public CategoryGroupController(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    [HttpPost("Create")]
    [Authorize(Policy = "Catalog_Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryGroupCommand request)
    {
        if (string.IsNullOrEmpty(request.Name))
            return BadRequest("Name cannot be null or empty");

        await _productDbContext.CategoryGroup.AddAsync(new Domain.CategoryGroupEntity()
        {
            Name = request.Name
        });

        await _productDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var categoryGroups = await _productDbContext.CategoryGroup
            .Include(c => c.Categories)
            .Select(categoryGroup => new GetCategoryGroupsCommandResponse.CategoryGroup
            {
                Name = categoryGroup.Name,
                Categories = categoryGroup.Categories.Select(category => new GetCategoryGroupsCommandResponse.CategoryGroup.Category()
                {
                    Id = category.Id,
                    Name = category.Name
                })
            })
            .ToListAsync();

        return Ok(new GetCategoryGroupsCommandResponse() { CategoryGroups =  categoryGroups });
    }
}

public class CreateCategoryGroupCommand
{
    public string Name { get; set; }
}

public class GetCategoryGroupsCommandResponse
{
    public List<CategoryGroup> CategoryGroups { get; set; }

    public class CategoryGroup
    {
        public string Name { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public class Category
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}