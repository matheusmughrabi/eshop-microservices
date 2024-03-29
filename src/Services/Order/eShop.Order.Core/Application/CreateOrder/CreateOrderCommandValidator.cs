﻿using eShop.Order.Core.Application.Utils;

namespace eShop.Order.Core.Application.CreateOrder;

public class CreateOrderCommandValidator : ICommandValidator<CreateOrderCommand, CreateOrderCommandResult>
{
    public ValidationResult Validate(CreateOrderCommand command)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrEmpty(command.UserId))
            validationResult.AddValidation("UserId", "userId cannot be null");

        if (command.Products is null || command.Products.Count == 0)
            validationResult.AddValidation("Products", "Order must have at least one product");

        if (command.Products.Any(c => c.PriceAtPurchase <= 0))
            validationResult.AddValidation("PriceAtPurchase", "All products must have a PriceAtPurchase greater than zero");

        if (command.Products.Any(c => c.Quantity <= 0))
            validationResult.AddValidation("Quantity", "All products must have a Quantity greater than zero");

        return validationResult;
    }
}
