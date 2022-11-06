using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpGet("GetPaginated/{CategoryId}")]
        public async Task<IActionResult> GetPaginated([FromQuery] GetProductsPaginatedQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetProductsPaginatedQuery : IRequest<GetProductsPaginatedQueryResponse>
    {
        [FromRoute]
        public Guid CategoryId { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Page must be an integer equal or greater than 1")]
        public int Page { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "ItemsPerPage must be an integer equal or greater than 1")]
        public int ItemsPerPage { get; set; }
    }

    public class GetProductsPaginatedQueryResponse
    {
        public List<Product> Products { get; set; }

        public class Product
        {
            public string Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
        }
    }

    public class GetProductsPaginatedQueryHandler : IRequestHandler<GetProductsPaginatedQuery, GetProductsPaginatedQueryResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public GetProductsPaginatedQueryHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<GetProductsPaginatedQueryResponse> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.ItemsPerPage;

            var products = await _productDbContext.Product
                .AsNoTracking()
                .Skip(skip)
                .Take(request.ItemsPerPage)
                .Select(c => new GetProductsPaginatedQueryResponse.Product()
                {
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price
                })
                .ToListAsync();

            return new GetProductsPaginatedQueryResponse() { Products = products };
        }
    }
}
