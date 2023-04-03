namespace eShop.Order.Core.Domain.Enums;

public enum OrderStatusEnum
{
    Processing = 1,
    Invalid = 2,
    Placed = 3,
    Shipped = 4,
    Completed = 4
}

public static class OrderStatusEnumExtensions
{
    public static string ToDescription(this OrderStatusEnum orderStatusEnum)
    {
        switch (orderStatusEnum)
        {
            case OrderStatusEnum.Processing: return "Processing";
            case OrderStatusEnum.Invalid: return "Invalid";
            case OrderStatusEnum.Placed: return "Placed";
            case OrderStatusEnum.Shipped: return "Shipped";
            case OrderStatusEnum: return "Completed";
        }
    }
}
