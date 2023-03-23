using eShop.OrderingApi.Application.Utils;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommandResult
{
    public bool Success { get; set; }
    public ValidationResult ValidationResult { get; set; }
}
