using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly PlaceOrderCommandValidator _placeOrderCommandValidator;

    public PlaceOrderCommandHandler(
        IOrderRepository orderRepository, 
        PlaceOrderCommandValidator placeOrderCommandValidator)
    {
        _orderRepository = orderRepository;
        _placeOrderCommandValidator = placeOrderCommandValidator;
    }

    public async Task<PlaceOrderCommandResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _placeOrderCommandValidator.Validate(request);
        if (validationResult.IsInvalid)
            return new PlaceOrderCommandResult() { Success = false, ValidationResult = validationResult }; 

        var orderEntity = BuildOrder(request);

        await _orderRepository.InsertAsync(orderEntity);

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
            orderEntity.AddProduct(new OrderEntity.Product(product.Id, product.Name, product.PriceAtPurchase, product.Quantity));
        }

        return orderEntity;
    }
}
