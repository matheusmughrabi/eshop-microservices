﻿using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Events.Publishers;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly CreateOrderCommandValidator _createOrderCommandValidator;
    private readonly OrderCreatedEventPublisher _orderCreatedEventPublisher;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        CreateOrderCommandValidator createOrderCommandValidator,
        OrderCreatedEventPublisher orderCreatedEventPublisher)
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

    private EventBus.Events.Messages.OrderCreatedEventMessage BuildOrderPlacedEventMessage(OrderEntity orderEntity)
    {
        return new EventBus.Events.Messages.OrderCreatedEventMessage()
        {
            OrderId = orderEntity.Id,
            Products = orderEntity.Products.Select(product => new EventBus.Events.Messages.OrderCreatedEventMessage.Product()
            {
                Id = product.Id,
                Quantity = product.Quantity
            }).ToList()
        };
    }
}