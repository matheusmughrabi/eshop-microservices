using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpGet("GetProductsWithoutCategory")]
        public async Task<IActionResult> GetProductsWithoutCategory([FromQuery] GetProductsWithoutCategoryPaginatedQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetProductsWithoutCategoryPaginatedQuery : IRequest<GetProductsWithoutCategoryResponse>
    {
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Page must be an integer equal or greater than 1")]
        public int Page { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "ItemsPerPage must be an integer equal or greater than 1")]
        public int ItemsPerPage { get; set; }
    }

    public class GetProductsWithoutCategoryResponse
    {
        public int TotalItems { get; set; }
        public List<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
        }
    }

    public class GetProductsWithoutCategoryPaginatedQueryHandler : IRequestHandler<GetProductsWithoutCategoryPaginatedQuery, GetProductsWithoutCategoryResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public GetProductsWithoutCategoryPaginatedQueryHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<GetProductsWithoutCategoryResponse> Handle(GetProductsWithoutCategoryPaginatedQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.ItemsPerPage;

            var products = await _productDbContext.Product
                .AsNoTracking()
                .Where(c => c.CategoryId == null)
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(request.ItemsPerPage)
                .Select(c => new GetProductsWithoutCategoryResponse.Product()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price
                })
                .ToListAsync();

            var totalItems = await _productDbContext.Product
                .AsNoTracking()
                .CountAsync(c => c.CategoryId == null);

            return new GetProductsWithoutCategoryResponse() { TotalItems = totalItems, Products = products };
        }
    }
}
