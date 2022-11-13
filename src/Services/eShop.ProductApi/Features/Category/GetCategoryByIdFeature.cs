using eShop.ProductApi.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.Features.Category
{
    public partial class CategoryController
    {
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetCategoryByIdQuery request) => Ok(await _mediator.Send(request));
    }

    public class GetCategoryByIdQuery : IRequest<GetCategoryByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
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
            return await _productDbContext.Category
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new GetCategoryByIdQueryResponse()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TotalProducts = c.Products.Count()
                })
                .FirstOrDefaultAsync();
        }
    }
}
