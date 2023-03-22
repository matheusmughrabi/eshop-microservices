using eShop.OrderingApi.Entity;

namespace eShop.OrderingApi.Repository;

public interface IOrderRepository
{
    Task InsertAsync(OrderEntity entity);
    Task<OrderEntity> GetByOrderIdAsync(string orderId);
    Task<OrderEntity> GetByUserIdAsync(string userId);
}
