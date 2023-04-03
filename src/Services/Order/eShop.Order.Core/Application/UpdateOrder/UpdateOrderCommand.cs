using eShop.Order.Core.Domain.Enums;
using MediatR;

namespace eShop.Order.Core.Application.UpdateOrder;

public class UpdateOrderCommand : IRequest<bool>
{
    public string Id { get; set; }
    public OrderStatusEnum Status { get; set; }
    public List<Notification> Notifications { get; set; }

    public class Notification
    {
        public string Description { get; set; }
    }
}
