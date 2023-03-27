using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace eShop.ProductApi.Features.CategoryGroup;

public partial class CategoryGroupController
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetCategoryGroupsQuery());

        return Ok(response);
    }
}

public class GetCategoryGroupsQuery : IRequest<GetCategoryGroupsQueryResponse>
{
}

public class GetCategoryGroupsQueryResponse
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

public class GetCategoryGroupsQueryHandler : IRequestHandler<GetCategoryGroupsQuery, GetCategoryGroupsQueryResponse>
{
    private readonly ProductDbContext _productDbContext;
    private readonly IDistributedCache _cache;

    public GetCategoryGroupsQueryHandler(ProductDbContext productDbContext, IDistributedCache cache)
    {
        _productDbContext = productDbContext;
        _cache = cache;
    }

    public async Task<GetCategoryGroupsQueryResponse> Handle(GetCategoryGroupsQuery request, CancellationToken cancellationToken)
    {
        const string recordId = "allCategoryGroups";
        var categoryGroups = await _cache.GetRecordAsync<List<GetCategoryGroupsQueryResponse.CategoryGroup>>(recordId);

        if (categoryGroups is null)
        {
            categoryGroups = await _productDbContext.CategoryGroup
               .Include(c => c.Categories)
               .Select(categoryGroup => new GetCategoryGroupsQueryResponse.CategoryGroup
               {
                   Name = categoryGroup.Name,
                   Categories = categoryGroup.Categories.Select(category => new GetCategoryGroupsQueryResponse.CategoryGroup.Category()
                   {
                       Id = category.Id,
                       Name = category.Name
                   })
               })
               .ToListAsync();

            await _cache.SetRecordAsync<List<GetCategoryGroupsQueryResponse.CategoryGroup>>(recordId, categoryGroups, TimeSpan.FromHours(24)); // Very long time span since there aren't gonna be that many Updates in the catalog...
        }

        return new GetCategoryGroupsQueryResponse() { CategoryGroups = categoryGroups };
    }
}
