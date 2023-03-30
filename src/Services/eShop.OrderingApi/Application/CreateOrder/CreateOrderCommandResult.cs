using eShop.OrderingApi.Application.Utils;

namespace eShop.OrderingApi.Application.CreateOrder;

public class CreateOrderCommandResult
{
    public bool Success { get; set; }
    public ValidationResult ValidationResult { get; set; }
}
