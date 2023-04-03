using MediatR;

namespace eShop.Order.Core.Application.GetOrders;

public class GetOrdersCommand : IRequest<GetOrdersCommandResult>
{
    public string UserId { get; set; }
}
