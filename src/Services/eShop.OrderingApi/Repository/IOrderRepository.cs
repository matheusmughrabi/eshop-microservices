using eShop.OrderingApi.Entity;

namespace eShop.OrderingApi.Repository;

public interface IOrderRepository
{
    Task InsertAsync(OrderEntity entity);
    Task<OrderEntity> GetByOrderIdAsync(string orderId);
    Task<List<OrderEntity>> GetByUserIdAsync(string userId);
    Task<bool> Update(OrderEntity entity);
}
