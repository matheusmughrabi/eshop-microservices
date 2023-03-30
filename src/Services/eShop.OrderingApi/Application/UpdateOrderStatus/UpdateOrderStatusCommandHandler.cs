using eShop.OrderingApi.Events.Publishers;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderPlacedEventPublisher _orderPlacedEventPublisher;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, OrderPlacedEventPublisher orderPlacedEventPublisher)
    {
        _orderRepository = orderRepository;
        _orderPlacedEventPublisher = orderPlacedEventPublisher;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var success = await _orderRepository.UpdateStatus(request.Id, request.Status);

        if (request.Status == Domain.Enums.OrderStatusEnum.Placed)
        {
            var order = await _orderRepository.GetByOrderIdAsync(request.Id);

            _orderPlacedEventPublisher.Publish(new EventBus.Events.Messages.OrderPlacedEventMessage()
            {
                OrderId = request.Id,
                Products = order.Products.Select(c => new EventBus.Events.Messages.OrderPlacedEventMessage.Product()
                {
                    Id = c.Id,
                    Quantity = c.Quantity
                }).ToList()
            });
        }  

        return success; 
    }
}
