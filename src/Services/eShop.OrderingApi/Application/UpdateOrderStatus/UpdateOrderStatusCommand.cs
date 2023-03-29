using eShop.OrderingApi.Domain.Enums;
using MediatR;

namespace eShop.OrderingApi.Application.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<bool>
{
    public string Id { get; set; }
    public OrderStatusEnum Status { get; set; }
}
