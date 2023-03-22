using eShop.OrderingApi.Entity;
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

    public Task<OrderEntity> GetByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task InsertAsync(OrderEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _orderCollection.InsertOneAsync(entity);
    }
}
