using eShop.ProductApi.DataAccess;
using eShop.ProductApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace eShop.ProductApi.Features.CategoryGroup;

public partial class CategoryGroupController
{
    [HttpGet("GetSummary")]
    public async Task<IActionResult> GetCategoryGroupsSummary()
    {
        var response = await _mediator.Send(new GetCategoryGroupsSummaryQuery());

        return Ok(response);
    }
}

public class GetCategoryGroupsSummaryQuery : IRequest<GetCategoryGroupsSummaryQueryResponse>
{
}

public class GetCategoryGroupsSummaryQueryResponse
{
    public List<CategoryGroup> CategoryGroups { get; set; }

    public class CategoryGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

public class GetCategoryGroupsSummaryHandler : IRequestHandler<GetCategoryGroupsSummaryQuery, GetCategoryGroupsSummaryQueryResponse>
{
    private readonly ProductDbContext _productDbContext;
    private readonly IDistributedCache _cache;

    public GetCategoryGroupsSummaryHandler(ProductDbContext productDbContext, IDistributedCache cache)
    {
        _productDbContext = productDbContext;
        _cache = cache;
    }

    public async Task<GetCategoryGroupsSummaryQueryResponse> Handle(GetCategoryGroupsSummaryQuery request, CancellationToken cancellationToken)
    {
        const string recordId = "categoryGroupsSummary";
        var categoryGroups = await _cache.GetRecordAsync<List<GetCategoryGroupsSummaryQueryResponse.CategoryGroup>>(recordId);

        if (categoryGroups is null)
        {
            categoryGroups = await _productDbContext.CategoryGroup
               .Select(categoryGroup => new GetCategoryGroupsSummaryQueryResponse.CategoryGroup
               {
                   Id = categoryGroup.Id,
                   Name = categoryGroup.Name
               })
               .ToListAsync();

            await _cache.SetRecordAsync<List<GetCategoryGroupsSummaryQueryResponse.CategoryGroup>>(recordId, categoryGroups, TimeSpan.FromHours(24)); // Very long time span since there aren't gonna be that many Updates in the catalog...
        }

        return new GetCategoryGroupsSummaryQueryResponse() { CategoryGroups = categoryGroups };
    }
}
