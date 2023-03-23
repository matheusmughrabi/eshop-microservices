namespace eShop.OrderingApi.Domain.Enums;

public enum OrderStatusEnum
{
    Placed = 1,
    Shipped = 2,
    Completed = 3
}

public static class OrderStatusEnumExtensions
{
    public static string ToDescription(this OrderStatusEnum orderStatusEnum)
    {
        switch (orderStatusEnum)
        {
            case OrderStatusEnum.Placed: return "Placed";
            case OrderStatusEnum.Shipped: return "Shipped";
            case OrderStatusEnum: return "Completed";
        }
    }
}
