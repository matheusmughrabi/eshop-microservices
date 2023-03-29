using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Events.Publishers;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly PlaceOrderCommandValidator _placeOrderCommandValidator;
    private readonly OrderPlacedEventPublisher _orderPlacedEventPublisher;

    public PlaceOrderCommandHandler(
        IOrderRepository orderRepository,
        PlaceOrderCommandValidator placeOrderCommandValidator,
        OrderPlacedEventPublisher orderPlacedEventPublisher)
    {
        _orderRepository = orderRepository;
        _placeOrderCommandValidator = placeOrderCommandValidator;
        _orderPlacedEventPublisher = orderPlacedEventPublisher;
    }

    public async Task<PlaceOrderCommandResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _placeOrderCommandValidator.Validate(request);
        if (validationResult.IsInvalid)
            return new PlaceOrderCommandResult() { Success = false, ValidationResult = validationResult }; 

        var orderEntity = BuildOrder(request);

        await _orderRepository.InsertAsync(orderEntity);

        var eventMessage = BuildOrderPlacedEventMessage(orderEntity);
        _orderPlacedEventPublisher.Publish(eventMessage);

        return new PlaceOrderCommandResult()
        {
            Success = true
        };

    }

    private OrderEntity BuildOrder(PlaceOrderCommand request)
    {
        var orderEntity = new OrderEntity(request.UserId);

        foreach (var product in request.Products)
        {
            orderEntity.AddProduct(new OrderEntity.Product(product.Id, product.Name, product.PriceAtPurchase, product.Quantity, product.ImagePath));
        }

        return orderEntity;
    }

    private EventBus.Events.Messages.OrderPlacedEventMessage BuildOrderPlacedEventMessage(OrderEntity orderEntity)
    {
        return new EventBus.Events.Messages.OrderPlacedEventMessage()
        {
            OrderId = orderEntity.Id,
            Products = orderEntity.Products.Select(product => new EventBus.Events.Messages.OrderPlacedEventMessage.Product()
            {
                Id = product.Id,
                Quantity = product.Quantity
            }).ToList()
        };
    }
}
