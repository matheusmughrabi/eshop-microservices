using MediatR;

namespace eShop.OrderingApi.Application.GetOrders;

public class GetOrdersCommand : IRequest<GetOrdersCommandResult>
{
    public string UserId { get; set; }
}
