using eShop.EventBus.Messages;
using eShop.OrderingApi.Events.Publishers;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.UpdateOrder;

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
                order.AddNotification(new Entity.OrderEntity.Notification() { Description = notification.Description });
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
