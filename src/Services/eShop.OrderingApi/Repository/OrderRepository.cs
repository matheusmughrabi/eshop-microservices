using eShop.OrderingApi.Domain.Enums;
using eShop.OrderingApi.Entity;
using eShop.OrderingApi.Repository.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eShop.OrderingApi.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderEntity> _orderCollection;

    public OrderRepository(IMongoClient client)
    {
        string databaseName = "OrderDb";
        string collectionName = "OrderCollection";

        var db = client.GetDatabase(databaseName);
        _orderCollection = db.GetCollection<OrderEntity>(collectionName);
    }

    public Task<OrderEntity> GetByOrderIdAsync(string orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<OrderEntity>> GetByUserIdAsync(string userId)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(c => c.UserId, userId);

        var orders = await _orderCollection.Find(filter).ToListAsync();

        return orders;
    }

    public async Task InsertAsync(OrderEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _orderCollection.InsertOneAsync(entity);
    }

    public async Task<bool> UpdateStatus(string id, OrderStatusEnum status)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(c => c.Id, id);
        var update = Builders<OrderEntity>.Update.Set(c => c.Status, status);
        var result = await _orderCollection.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }
}
