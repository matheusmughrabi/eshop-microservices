using eShop.EventBus.Messages;
using eShop.Order.Core.Domain.Entity;
using eShop.Order.Core.Events.Publishers;
using eShop.Order.Core.Repository;
using MediatR;

namespace eShop.Order.Core.Application.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly CreateOrderCommandValidator _createOrderCommandValidator;
    private readonly CheckStockCommandPublisher _orderCreatedEventPublisher;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        CreateOrderCommandValidator createOrderCommandValidator,
        CheckStockCommandPublisher orderCreatedEventPublisher)
    {
        _orderRepository = orderRepository;
        _createOrderCommandValidator = createOrderCommandValidator;
        _orderCreatedEventPublisher = orderCreatedEventPublisher;
    }

    public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _createOrderCommandValidator.Validate(request);
        if (validationResult.IsInvalid)
            return new CreateOrderCommandResult() { Success = false, ValidationResult = validationResult };

        var orderEntity = BuildOrder(request);

        await _orderRepository.InsertAsync(orderEntity);

        var eventMessage = BuildOrderPlacedEventMessage(orderEntity);
        _orderCreatedEventPublisher.Publish(eventMessage);

        return new CreateOrderCommandResult()
        {
            Success = true
        };

    }

    private OrderEntity BuildOrder(CreateOrderCommand request)
    {
        var orderEntity = new OrderEntity(request.UserId);

        foreach (var product in request.Products)
        {
            orderEntity.AddProduct(new OrderEntity.Product(product.Id, product.Name, product.PriceAtPurchase, product.Quantity, product.ImagePath));
        }

        return orderEntity;
    }

    private CheckStockCommandMessage BuildOrderPlacedEventMessage(OrderEntity orderEntity)
    {
        return new EventBus.Messages.CheckStockCommandMessage()
        {
            OrderId = orderEntity.Id,
            Products = orderEntity.Products.Select(product => new CheckStockCommandMessage.Product()
            {
                Id = product.Id,
                Quantity = product.Quantity
            }).ToList()
        };
    }
}
