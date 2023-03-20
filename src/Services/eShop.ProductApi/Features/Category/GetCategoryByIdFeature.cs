using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById([FromQuery] GetCategoryByIdQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetCategoryByIdQuery : IRequest<GetCategoryByIdQueryResponse>
    {
        [FromRoute]
        public Guid Id { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Page must be an integer equal or greater than 1")]
        public int Page { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "ItemsPerPage must be an integer equal or greater than 1")]
        public int ItemsPerPage { get; set; }
    }

    public class GetCategoryByIdQueryResponse
    {
        public int TotalItems { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
        public List<Product> Products { get; set; }

        public class Product
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string ImagePath { get; set; }
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public GetCategoryByIdQueryHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<GetCategoryByIdQueryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.ItemsPerPage;

            return await _productDbContext.Category
                .AsNoTracking()
                .Include(i => i.Products)
                .Where(c => c.Id == request.Id)
                .Select(c => new GetCategoryByIdQueryResponse()
                {
                    TotalItems = c.Products.Count(),
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TotalProducts = c.Products.Count(),
                    Products = c.Products.Skip(skip).Take(request.ItemsPerPage).Select(p => new GetCategoryByIdQueryResponse.Product()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImagePath = p.ImagePath
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
