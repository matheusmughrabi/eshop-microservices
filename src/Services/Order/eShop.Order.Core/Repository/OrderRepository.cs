﻿using eShop.Order.Core.Domain.Entity;
using MongoDB.Driver;

namespace eShop.Order.Core.Repository;

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

    public async Task<OrderEntity> GetByOrderIdAsync(string orderId)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(c => c.Id, orderId);

        var order = await _orderCollection.Find(filter).FirstOrDefaultAsync();

        return order;
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

    public async Task<bool> Update(OrderEntity entity)
    {
        var filter = Builders<OrderEntity>.Filter.Eq(c => c.Id, entity.Id);
        var result = await _orderCollection.ReplaceOneAsync(filter, entity);

        return result.ModifiedCount > 0;
    }
}
