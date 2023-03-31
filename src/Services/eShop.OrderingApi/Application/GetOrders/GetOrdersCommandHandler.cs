using eShop.OrderingApi.Domain.Enums;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.GetOrders;

public class GetOrdersCommandHandler : IRequestHandler<GetOrdersCommand, GetOrdersCommandResult>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrdersCommandResult> Handle(GetOrdersCommand request, CancellationToken cancellationToken)
    {
        var response = await _orderRepository.GetByUserIdAsync(request.UserId);

        return new GetOrdersCommandResult()
        {
            Orders = response.Select(c => new GetOrdersCommandResult.Order
            {
                Id = c.Id,
                DateOfPurchase = c.DateOfPurchase,
                Status = c.Status,
                StatusDescription = c.Status.ToDescription(),

                Products = c.Products.Select(p => new GetOrdersCommandResult.Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    PriceAtPurchase = p.PriceAtPurchase,
                    Quantity = p.Quantity,
                    ImagePath = p.ImagePath
                }).ToList(),

                Notifications = c.Notifications?.Select(p => new GetOrdersCommandResult.Notification()
                {
                    Description = p.Description
                }).ToList()

            }).ToList()
        };
    }
}
