using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product;

public partial class ProductController
{
    [HttpGet("GetProducts")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery request) => Ok(await _mediator.Send(request));
}

public class GetProductsQuery : IRequest<GetProductsQueryResponse>
{
    [FromQuery]
    public Guid? CategoryId { get; set; }
}

public class GetProductsQueryResponse
{
    public List<Product> Products { get; set; }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsQueryResponse>
{
    private readonly ProductDbContext _productDbContext;

    public GetProductsQueryHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<GetProductsQueryResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _productDbContext
            .Product
            .AsNoTracking()
            .Where(c => request.CategoryId == null || c.CategoryId == request.CategoryId)
            .Select(c => new GetProductsQueryResponse.Product()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImagePath = c.ImagePath,
                Price = c.Price,
                CategoryId = c.CategoryId
            })
            .OrderBy(c => c.CategoryId)
            .ToList();

        return new GetProductsQueryResponse() { Products = products };
    }
}
