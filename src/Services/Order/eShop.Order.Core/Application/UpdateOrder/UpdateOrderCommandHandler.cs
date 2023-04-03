using eShop.EventBus.Messages;
using eShop.Order.Core.Events.Publishers;
using eShop.Order.Core.Repository;
using MediatR;

namespace eShop.Order.Core.Application.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderPlacedEventPublisher _orderPlacedEventPublisher;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, OrderPlacedEventPublisher orderPlacedEventPublisher)
    {
        _orderRepository = orderRepository;
        _orderPlacedEventPublisher = orderPlacedEventPublisher;
    }

    public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderIdAsync(request.Id);
        order.Status = request.Status;

        if (request.Notifications?.Count > 0)
        {
            foreach (var notification in request.Notifications)
            {
                order.AddNotification(new Domain.Entity.OrderEntity.Notification() { Description = notification.Description });
            }
        }

        var success = await _orderRepository.Update(order);

        if (request.Status == Domain.Enums.OrderStatusEnum.Placed)
        {
            _orderPlacedEventPublisher.Publish(new EventBus.Messages.OrderPlacedEventMessage()
            {
                OrderId = request.Id,
                Products = order.Products.Select(c => new OrderPlacedEventMessage.Product()
                {
                    Id = c.Id,
                    Quantity = c.Quantity
                }).ToList()
            });
        }

        return success;
    }
}
