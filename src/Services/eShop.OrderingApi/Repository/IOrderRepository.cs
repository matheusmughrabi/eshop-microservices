using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Repository.Models;

namespace eShop.OrderingApi.Repository;

public interface IOrderRepository
{
    Task InsertAsync(OrderEntity entity);
    Task<OrderEntity> GetByOrderIdAsync(string orderId);
    Task<List<OrderEntity>> GetByUserIdAsync(string userId);
}
