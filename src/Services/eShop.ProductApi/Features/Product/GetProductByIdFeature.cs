using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Product
{
    public partial class ProductController
    {
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetProductByIdQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetProductByIdQuery : IRequest<GetProductByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityOnHand { get; set; }
        public string ImagePath { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
    {
        private readonly ProductDbContext _productDbContext;

        public GetProductByIdQueryHandler(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productDbContext.Product
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new GetProductByIdQueryResponse()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    QuantityOnHand = c.QuantityOnHand,
                    ImagePath = c.ImagePath,
                    CategoryId = c.Category.Id
                })
                .FirstOrDefaultAsync();
        }
    }
}
