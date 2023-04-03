using eShop.Order.Core.Application.Utils;

namespace eShop.Order.Core.Application.CreateOrder;

public class CreateOrderCommandResult
{
    public bool Success { get; set; }
    public ValidationResult ValidationResult { get; set; }
}
