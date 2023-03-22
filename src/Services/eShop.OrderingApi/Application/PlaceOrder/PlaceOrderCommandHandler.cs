using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Repository;
using MediatR;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResponse>
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

    public async Task<PlaceOrderCommandResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _placeOrderCommandValidator.Validate(request);
        if (validationResult.IsInvalid)
            return new PlaceOrderCommandResponse() { Success = false, ValidationResult = validationResult }; 

        var orderEntity = MapRequest(request);

        await _orderRepository.InsertAsync(orderEntity);

        return new PlaceOrderCommandResponse()
        {
            Success = true
        };

    }

    private OrderEntity MapRequest(PlaceOrderCommand request)
    {
        return new OrderEntity()
        {
            UserId = request.UserId,
            DateOfPurchase = DateTime.UtcNow,
            Products = request.Products.Select(c => new OrderEntity.Product()
            {
                Id = c.Id,
                PriceAtTimeOfOrder = c.PriceAtPurchase,
                Quantity = c.Quantity
            }).ToList()
        };
    }
}
