using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpGet("GetPaginated")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaginated([FromQuery] GetCategoriesPaginatedQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetCategoriesPaginatedQuery : IRequest<GetCategoriesPaginatedQueryResponse>
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public bool Paginate { get; set; }
    }

    public class GetCategoriesPaginatedQueryResponse
    {
        public int TotalItems { get; set; }
        public List<Category> Categories { get; set; }

        public class Category
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int TotalProducts { get; set; }
        }
    }

    public class GetCategoriesPaginatedQueryHandler : IRequestHandler<GetCategoriesPaginatedQuery, GetCategoriesPaginatedQueryResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public GetCategoriesPaginatedQueryHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<GetCategoriesPaginatedQueryResponse> Handle(GetCategoriesPaginatedQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.ItemsPerPage;

            var categoriesQuery = _productDbContext.Category
                .AsNoTracking();

            if (request.Paginate)
            {
                categoriesQuery = categoriesQuery
                    .Skip(skip)
                    .Take(request.ItemsPerPage);
            }

            var categories = await categoriesQuery
                .OrderBy(c => c.Name)
                .Select(c => new GetCategoriesPaginatedQueryResponse.Category()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TotalProducts = c.Products.Count()
                })
                .ToListAsync();

            var totalItems = await _productDbContext.Category
                .AsNoTracking()
                .CountAsync();

            return new GetCategoriesPaginatedQueryResponse() { TotalItems = totalItems, Categories = categories };
        }
    }
}
