using eShop.OrderingApi.Application.Utils;

namespace eShop.OrderingApi.Application.PlaceOrder;

public class PlaceOrderCommandValidator : ICommandValidator<PlaceOrderCommand, PlaceOrderCommandResponse>
{
    public ValidationResult Validate(PlaceOrderCommand command)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrEmpty(command.UserId))
            validationResult.AddValidation("UserId", "userId cannot be null");

        if (command.Products is null || command.Products.Count == 0)
            validationResult.AddValidation("Products", "Order must have at least one product");
        
        if (command.Products.Any(c => c.Quantity <= 0))
            validationResult.AddValidation("Quantity", "All products must have a Quantity greater than zero");

        return validationResult;
    }
}
