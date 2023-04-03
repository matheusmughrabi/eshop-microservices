using eShop.Order.Core.Domain.Entity;

namespace eShop.Order.Core.Repository;

public interface IOrderRepository
{
    Task InsertAsync(OrderEntity entity);
    Task<OrderEntity> GetByOrderIdAsync(string orderId);
    Task<List<OrderEntity>> GetByUserIdAsync(string userId);
    Task<bool> Update(OrderEntity entity);
}
